namespace AshBoard.Application.DTOs.Alerta
{
    public class UpdateAlertaDto
    {
        public string Gravidade { get; set; } = "Verde";
        public bool IncendioProximo { get; set; }
    }
}
