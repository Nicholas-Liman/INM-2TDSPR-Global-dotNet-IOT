using SensorEmulator.Services;
using System.Net.Http.Headers;

var http = new HttpClient
{
    BaseAddress = new Uri("https://localhost:5001")
};

http.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/json"));

var simulador = new SensorSimuladorService(http);

Console.WriteLine("Iniciando simulação de sensores...\n");

while (true)
{
    await simulador.ExecutarSimulacaoAsync();
    await Task.Delay(5000); // Aguarda 5 segundos entre cada rodada de leitura
}
