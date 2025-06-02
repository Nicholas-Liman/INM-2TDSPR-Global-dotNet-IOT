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
                var resultado = Predict(temperatura, co2);

                if (resultado == null)
                {
                    Console.WriteLine("⚠️ Predição retornou null.");
                    return 0f;
                }

                Console.WriteLine($"[ML DEBUG] Resultado: Label={resultado.PredictedLabel}, Prob={resultado.Probability}, Score={resultado.Score}");

                return (float)Math.Round(resultado.Probability * 100f, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro na predição: {ex.Message}");
                return 0f;
            }
        }
    }

}
