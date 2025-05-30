namespace AshBoard.Application.Interfaces
{
    public interface IAlertaMLService
    {
        float ObterProbabilidadeIncendio(float temperatura, float co2);
    }
}
