using AshBoard.Application.Interfaces;
using static AshBoard.Data.ML.MLModel;

namespace AshBoard.Service.Services
{
    public class AlertaMLService : IAlertaMLService
    {
        public float ObterProbabilidadeIncendio(float temperatura, float co2)
        {
            var input = new ModelInput
            {
                Temperatura = temperatura,
                NivelCO2 = co2
            };

            var prediction = Predict(input);

            if (prediction.Score != null && prediction.Score.Length >= 2)
                return prediction.Score[1]; // probabilidade de incêndio
            else
                return 0f;
        }
    }
}
