using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;

namespace AshBoard.Data.ML
{
    public partial class MLModel
    {
        /// <summary>
        /// Caminho do modelo treinado (.zip)
        /// </summary>
        private static readonly string MLNetModelPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "ML", "MLModel.zip"
        );

        #region model input class
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
        #endregion

        #region model output class
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

        /// <summary>
        /// Prediction engine gerenciado via lazy loading
        /// </summary>
        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine =
            new(() => CreatePredictionEngine(), true);

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictionEngine()
        {
            var mlContext = new MLContext();

            if (!File.Exists(MLNetModelPath))
            {
                throw new FileNotFoundException($"❌ Modelo não encontrado no caminho: {MLNetModelPath}");
            }

            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        /// <summary>
        /// Predição usando ModelInput diretamente
        /// </summary>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }

        /// <summary>
        /// Predição com entrada direta (float temperatura e CO2)
        /// </summary>
        public static ModelOutput Predict(float temperatura, float co2)
        {
            return Predict(new ModelInput
            {
                Temperatura = temperatura,
                NivelCO2 = co2
            });
        }
    }
}
