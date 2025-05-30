// MLModel.cs
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace AshBoard.Data.ML
{
    public partial class MLModel
    {
        // Caminho do arquivo .zip do modelo
        private static string MLNetModelPath = Path.Combine(AppContext.BaseDirectory, "AppData", "MLModel.zip");

        // Classes de entrada e saída do modelo
        #region InputOutput
        public class ModelInput
        {
            [LoadColumn(0)]
            [ColumnName("Temperatura")]
            public float Temperatura { get; set; }

            [LoadColumn(1)]
            [ColumnName("NivelCO2")]
            public float NivelCO2 { get; set; }

            [LoadColumn(2)]
            [ColumnName("Incendio")]
            public string Incendio { get; set; }
        }

        public class ModelOutput
        {
            [ColumnName("Temperatura")]
            public float Temperatura { get; set; }

            [ColumnName("NivelCO2")]
            public float NivelCO2 { get; set; }

            [ColumnName("Incendio")]
            public uint Incendio { get; set; }

            [ColumnName("Features")]
            public float[] Features { get; set; }

            [ColumnName("PredictedLabel")]
            public string PredictedLabel { get; set; }

            [ColumnName("Score")]
            public float[] Score { get; set; }
        }
        #endregion

        // Predição padrão
        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine =
            new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }

        // Obter todas as pontuações
        public static IOrderedEnumerable<KeyValuePair<string, float>> PredictAllLabels(ModelInput input)
        {
            var result = Predict(input);
            return GetSortedScoresWithLabels(result);
        }

        public static IOrderedEnumerable<KeyValuePair<string, float>> GetSortedScoresWithLabels(ModelOutput result)
        {
            var scores = result.Score;
            var labels = GetLabels(result);

            var mapped = labels.Zip(scores, (label, score) => new KeyValuePair<string, float>(label, score));
            return mapped.OrderByDescending(x => x.Value);
        }

        private static IEnumerable<string> GetLabels(ModelOutput result)
        {
            var schema = PredictEngine.Value.OutputSchema;
            var labelColumn = schema.GetColumnOrNull("Incendio");

            if (labelColumn == null)
                throw new Exception("Coluna 'Incendio' não encontrada no modelo.");

            var keyNames = new VBuffer<ReadOnlyMemory<char>>();
            labelColumn.Value.GetKeyValues(ref keyNames);
            return keyNames.DenseValues().Select(x => x.ToString());
        }

        // Caminho para re-treinamento
        public const string RetrainFilePath = @"D:\Downloads\INM-2TDSPR-Global-dotNet-IOT-main\dataset_incendios.csv";
        public const char RetrainSeparatorChar = ',';
        public const bool RetrainHasHeader = true;
        public const bool RetrainAllowQuoting = false;

        public static void Train(string outputModelPath, string inputDataFilePath = RetrainFilePath, char separatorChar = RetrainSeparatorChar, bool hasHeader = RetrainHasHeader, bool allowQuoting = RetrainAllowQuoting)
        {
            var mlContext = new MLContext();
            var data = LoadIDataViewFromFile(mlContext, inputDataFilePath, separatorChar, hasHeader, allowQuoting);
            var model = RetrainModel(mlContext, data);
            SaveModel(mlContext, model, data, outputModelPath);
        }

        public static IDataView LoadIDataViewFromFile(MLContext mlContext, string inputDataFilePath, char separatorChar, bool hasHeader, bool allowQuoting)
        {
            return mlContext.Data.LoadFromTextFile<ModelInput>(inputDataFilePath, separatorChar, hasHeader, allowQuoting: allowQuoting);
        }

        public static void SaveModel(MLContext mlContext, ITransformer model, IDataView data, string modelSavePath)
        {
            using var fs = File.Create(modelSavePath);
            mlContext.Model.Save(model, data.Schema, fs);
        }

        public static ITransformer RetrainModel(MLContext mlContext, IDataView trainData)
        {
            var pipeline = BuildPipeline(mlContext);
            return pipeline.Fit(trainData);
        }

        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            var pipeline = mlContext.Transforms
                .ReplaceMissingValues(new[]
                {
                    new InputOutputColumnPair("Temperatura", "Temperatura"),
                    new InputOutputColumnPair("NivelCO2", "NivelCO2")
                })
                .Append(mlContext.Transforms.Concatenate("Features", "Temperatura", "NivelCO2"))
                .Append(mlContext.Transforms.Conversion.MapValueToKey("Incendio"))
                .Append(mlContext.MulticlassClassification.Trainers
                    .OneVersusAll(
                        mlContext.BinaryClassification.Trainers
                            .LbfgsLogisticRegression(new LbfgsLogisticRegressionBinaryTrainer.Options
                            {
                                L1Regularization = 1f,
                                L2Regularization = 1f,
                                LabelColumnName = "Incendio",
                                FeatureColumnName = "Features"
                            }),
                        labelColumnName: "Incendio"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            return pipeline;
        }

        public static void TrainAndSaveAsZip(string outputZipPath)
        {
            var mlContext = new MLContext();

            var data = LoadIDataViewFromFile(
                mlContext,
                RetrainFilePath,
                RetrainSeparatorChar,
                RetrainHasHeader,
                RetrainAllowQuoting
            );

            var model = RetrainModel(mlContext, data);

            // Salvar o modelo como .zip na raiz
            mlContext.Model.Save(model, data.Schema, outputZipPath);
        }

    }
}
