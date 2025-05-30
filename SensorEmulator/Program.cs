using SensorEmulator.Services;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://localhost:7087")
};

var simulador = new SensorSimulatorService(httpClient);

Console.WriteLine("SensorEmulator iniciado...\n");

while (true)
{
    await simulador.AtualizarSensoresAsync();
    await Task.Delay(TimeSpan.FromSeconds(10));
}
