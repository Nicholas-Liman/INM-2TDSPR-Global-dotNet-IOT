using System;

namespace AshBoard.Application.DTOs.Alerta
{
    public class CreateAlertaDto
    {
        public DateTime DataHoraColeta { get; set; }
        public string NomeLocal { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Evento { get; set; } = "Incêndio";
        public string Gravidade { get; set; } = "Verde";
        public string SensorId { get; set; } = string.Empty;
        public string? Observacao { get; set; }

        // ML.NET
        public float Temperatura { get; set; }
        public float NivelCO2 { get; set; }
        public float ProbabilidadeIncendio { get; set; }
    }

}
