using AshBoard.Application.Interfaces;
using static AshBoard.Data.ML.MLModel;

namespace AshBoard.Service.Services
{
    public class AlertaMLService : IAlertaMLService
    {
        public float ObterProbabilidadeIncendio(float temperatura, float co2)
        {
            try
            {
                var input = new ModelInput
                {
                    Temperatura = temperatura,
                    NivelCO2 = co2
                };

                var prediction = Predict(input);

                // Arredonda para 2 casas decimais
                return (float)Math.Round(prediction.Probability * 100f, 2);
            }
            catch
            {
                // Em caso de erro de predição, retorna 0
                return 0f;
            }
        }
    }
}
