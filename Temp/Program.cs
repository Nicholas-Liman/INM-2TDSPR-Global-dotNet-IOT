using System;
using AshBoard.Data.ML;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Iniciando treinamento do modelo...");

        // Caminho absoluto desejado
        string outputPath = @"D:\Downloads\INM-2TDSPR-Global-dotNet-IOT-main\AshBoard.Data\ML\MLModel.zip";

        // Garante que o diretório exista
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

        // Treina e salva
        MLModel.TrainAndSaveAsZip(outputPath);

        Console.WriteLine("Modelo salvo com sucesso em:");
        Console.WriteLine(outputPath);
    }
}
