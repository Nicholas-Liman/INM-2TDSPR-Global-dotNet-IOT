﻿using System;

namespace AshBoard.Application.DTOs.Alerta
{
    public class AlertaDto
    {
        public int Id { get; set; }
        public DateTime DataHoraColeta { get; set; }
        public string NomeLocal { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Evento { get; set; } = "Incêndio";
        public string Gravidade { get; set; } = "Verde";
        public string SensorId { get; set; } = string.Empty;

        // Dados do modelo ML
        public float Temperatura { get; set; }
        public float NivelCO2 { get; set; }
        public float ProbabilidadeIncendio { get; set; }

        // Observação opcional (texto formatado)
        public string? Observacao { get; set; }
    }
}
