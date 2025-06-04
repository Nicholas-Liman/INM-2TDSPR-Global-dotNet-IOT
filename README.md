# 🔥 AshBoard – Monitoramento Inteligente de Incêndios

## 📌 Sobre o Projeto

Este projeto foi desenvolvido como parte da disciplina **Advanced Business Development with .NET**, com foco em **IoT**, **Machine Learning** e **boas práticas de APIs RESTful**, pela equipe:

👥 **Integrantes**
- 🏆 **Igor Gabriel Marcondes** – RM553544  
- 🏆 **Maria Beatriz Fogolin** – RM552669  
- 🏆 **Nicholas Barbosa Lima** – RM552744  

🎯 **Objetivo**  
Criar uma **API .NET escalável e inteligente** para monitoramento de sensores ambientais, com foco em **prevenção de incêndios florestais**, utilizando:

- CRUD completo de sensores, arrays e alertas  
- Classificação com **ML.NET** para prever risco de incêndio  
- Comunicação entre microsserviços com **RabbitMQ**  
- Documentação com Swagger  
- Testes com xUnit  
- Atualização em tempo real via chamadas assíncronas

---

## 🎥 Demonstração

📹 [Assista no YouTube](https://youtu.be/JK3yzIlKi_M)

---

## 🧱 Arquitetura

O projeto segue uma arquitetura organizada em camadas:

- `Domain` – Entidades e regras de domínio  
- `Application` – DTOs, serviços e interfaces  
- `Data` – Repositórios, contexto EF e ML  
- `Service` – Lógica de negócio  
- `API/Controllers` – Endpoints públicos com HATEOAS

---

## 🛠️ Tecnologias Utilizadas

- ✅ **ASP.NET Core Web API**  
- ✅ **Entity Framework Core**  
- ✅ **ML.NET (.zip customizado)**  
- ✅ **RabbitMQ (mensageria de alerta)**  
- ✅ **xUnit + Moq**  
- ✅ **Swagger (Swashbuckle)**

---

## ⚙️ Requisitos

- [.NET SDK 7.0+](https://dotnet.microsoft.com/en-us/download)
- RabbitMQ rodando localmente ou via Docker  
- Banco SQL Server LocalDB ou equivalente

---

## 🚀 Como Executar o Projeto
Antes de rodar certifique-se que o StartUp Item é Full Project
```bash
# Na raiz do projeto
dotnet restore    # Restaura dependências
dotnet build      # Compila o projeto
dotnet run        # Inicia a aplicação
````

> Acesse: `https://localhost:5001/swagger`

---

## 📡 Endpoints da API

### 📁 SensorController

| Método | Rota                           | Ação                    |
| ------ | ------------------------------ | ----------------------- |
| GET    | `/Dashboard/Index`             | Lista todos os sensores |
| POST   | `/Dashboard/CreateSensor`      | Cadastra novo sensor    |
| GET    | `/Dashboard/EditSensor/{id}`   | Edita sensor existente  |
| POST   | `/Dashboard/DeleteSensor/{id}` | Remove sensor           |
| GET    | `/Dashboard/ObterSensoresJson` | Leitura em tempo real   |

### 📁 ArraySensorController

| Método | Rota                                | Ação                           |
| ------ | ----------------------------------- | ------------------------------ |
| GET    | `/Dashboard/ArraySensors`           | Lista arrays de sensores       |
| POST   | `/Dashboard/CreateArraySensor`      | Cadastra novo array            |
| GET    | `/Dashboard/DeleteArraySensor/{id}` | Remove array                   |
| GET    | `/Dashboard/ObterArraysJson`        | Arrays com sensores + leituras |

### 📁 AlertaController

| Método | Rota                           | Ação                   |
| ------ | ------------------------------ | ---------------------- |
| GET    | `/Dashboard/Alertas`           | Lista de alertas       |
| POST   | `/Dashboard/CreateAlerta`      | Cria alerta com ML.NET |
| POST   | `/Dashboard/DeleteAlerta/{id}` | Remove alerta          |
| GET    | `/Dashboard/ObterAlertasJson`  | Alertas em tempo real  |

---

## 📊 Machine Learning

Utilizando **ML.NET**, o modelo é treinado para prever incêndios com base em:

* Temperatura
* Nível de CO₂

O modelo é treinado localmente e exportado como `MLModel.zip`. Ele é consumido via serviço `AlertaMLService` que calcula a probabilidade de incêndio e define alertas automáticos.

> 🔁 Também é possível re-treinar o modelo com `TrainAndSaveAsZip(...)` diretamente.

---

## 🧪 Testes com xUnit

Testes de unidade foram implementados para:

* Lógica de serviço (ex: `ArraySensorServiceTests`)
* Validações de criação e leitura
* Mock de repositórios com `Moq`

Rodar os testes:

```bash
dotnet test
```

---

## 🧠 Exemplo de JSON

### Criar Sensor

```json
{
  "id": "Sensor-1",
  "nomeLocal": "Parque Vila Lobos - Portão 3",
  "latitude": -23.512,
  "longitude": -46.623
}
```

### Criar ArraySensor

```json
{
  "nomeLocal": "Parque Vila Lobos",
  "sensorIds": [
    "Sensor-1",
    "Sensor-2",
    "Sensor-3"
  ]
}

```

### Criar Alerta
Alertas são criados automaticamente

## 📌 Conclusão

Este projeto demonstra como é possível unir tecnologias modernas como ASP.NET Core, ML.NET, RabbitMQ, e boas práticas de arquitetura RESTful para construir uma solução inteligente de monitoramento ambiental.

Com ele, conseguimos:

📡 Monitorar sensores de temperatura e CO₂ em tempo real

🔥 Detectar riscos de incêndio com auxílio de Machine Learning

⚙️ Automatizar respostas com alertas classificados por gravidade

🛠️ Gerenciar sensores e arrays com facilidade pela interface web

📊 Observar tudo em tempo real via painéis dinâmicos

🧪 Garantir qualidade com cobertura de testes usando xUnit

### 🧠 Desenvolvido por **Equipe B.I.N.**

