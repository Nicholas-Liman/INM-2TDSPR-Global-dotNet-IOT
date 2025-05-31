using System;
using AshBoard.Data.ML;
using Microsoft.ML;

namespace AshBoard.Data.ML
{
    public static class MLModelFacade
    {
        public static void Train() =>
            MLModel.Train("ML/MLModel.zip");

        public static MLModel.ModelOutput Predict(float temperatura, float co2) =>
            MLModel.Predict(temperatura, co2);

        public static void Evaluate()
        {
            var mlContext = new MLContext();

            // Caminho do modelo salvo
            var modelPath = "ML/MLModel.zip";
            var dataPath = MLModel.RetrainFilePath;

            if (!System.IO.File.Exists(modelPath))
            {
                Console.WriteLine($"❌ Modelo não encontrado em: {modelPath}");
                return;
            }

            if (!System.IO.File.Exists(dataPath))
            {
                Console.WriteLine($"❌ Dataset de avaliação não encontrado em: {dataPath}");
                return;
            }

            // Carrega modelo e dados
            ITransformer model = mlContext.Model.Load(modelPath, out _);
            var data = MLModel.LoadIDataViewFromFile(mlContext, dataPath, MLModel.RetrainSeparatorChar, MLModel.RetrainHasHeader, MLModel.RetrainAllowQuoting);

            // Avalia o modelo
            var predictions = model.Transform(data);
            var metrics = mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: "Incendio");

            // Exibe métricas no console
            Console.WriteLine("\n📊 Métricas de Avaliação:");
            Console.WriteLine($"Acurácia   : {metrics.Accuracy:P2}");
            Console.WriteLine($"F1 Score   : {metrics.F1Score:P2}");
            Console.WriteLine($"Precisão   : {metrics.PositivePrecision:P2}");
            Console.WriteLine($"Revocação  : {metrics.PositiveRecall:P2}");
            Console.WriteLine($"AUC        : {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine("=========================================");
        }
    }
}
