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
        // Caminho relativo corrigido para fora do bin
        private static readonly string MLNetModelPath =
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\AshBoard.Data\ML\MLModel.zip"));

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
            public bool Incendio { get; set; }
        }

        public class ModelOutput
        {
            [ColumnName("PredictedLabel")]
            public bool PredictedLabel { get; set; }

            [ColumnName("Probability")]
            public float Probability { get; set; }

            [ColumnName("Score")]
            public float Score { get; set; }
        }
        #endregion

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

        // Caminho para re-treinamento do modelo
        public const string RetrainFilePath = @"D:\Downloads\INM-2TDSPR-Global-dotNet-IOT-main\dataset_incendios_contextual_limpo.csv";
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
                .Append(mlContext.BinaryClassification.Trainers
                    .SdcaLogisticRegression(new SdcaLogisticRegressionBinaryTrainer.Options
                    {
                        LabelColumnName = "Incendio",
                        FeatureColumnName = "Features",
                        L1Regularization = 1f,
                        L2Regularization = 1f
                    }));

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

            // Garante que o diretório exista
            Directory.CreateDirectory(Path.GetDirectoryName(outputZipPath)!);

            mlContext.Model.Save(model, data.Schema, outputZipPath);
        }
    }
}
