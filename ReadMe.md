# CSharpQuizBlazor Solution

A multi-project solution for a C# Quiz web application built with Blazor WebAssembly, Azure Functions, and xUnit tests.

## Projects

### 1. CSharpQuizBlazor ([CSharpQuizBlazor/CSharpQuizBlazor.csproj](CSharpQuizBlazor/CSharpQuizBlazor.csproj))
- **Type:** Blazor WebAssembly App
- **Target:** .NET 9
- **Description:**
  - Interactive quiz app to test C# knowledge.
  - Features multiple-choice questions, instant feedback, and score calculation.
  - Results are posted to an Azure Function backend.
- **Key Files:**
  - `Pages/Quiz.razor`: Main quiz UI and logic.
  - `Services/QuizEngine.cs`: Quiz state and logic engine.
  - `Models/Question.cs`, `Models/QuizResultDto.cs`: Data models.

### 2. CSharpQuizFunc ([CSharpQuizFunc/CSharpQuizFunc.csproj](CSharpQuizFunc/CSharpQuizFunc.csproj))
- **Type:** Azure Functions (v4)
- **Target:** .NET 8
- **Description:**
  - Receives quiz results via HTTP POST and stores them in Azure Table Storage.
  - Example endpoint: `/api/Function1`
- **Key Files:**
  - `Function1.cs`: Main HTTP-triggered function for result storage.

### 3. CSharpQuizTest ([CSharpQuizTest/CSharpQuizTest.csproj](CSharpQuizTest/CSharpQuizTest.csproj))
- **Type:** xUnit Test Project
- **Target:** .NET 9
- **Description:**
  - Unit tests for quiz logic and state management.
- **Key Files:**
  - `QuizEngineTests.cs`: Tests for `QuizEngine`.

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js](https://nodejs.org/) (for some Blazor tooling, optional)
- Azure account (for deploying the function)

### Running the Quiz App
1. Open the solution in Visual Studio or VS Code.
2. Set `CSharpQuizBlazor` as the startup project.
3. Run the project. The app will launch in your browser at `https://localhost:xxxx/`.

### Running Tests
```
dotnet test CSharpQuizTest/CSharpQuizTest.csproj
```

### Deploying the Azure Function
- Update the Azure Table Storage connection string in your Azure Function App settings (`QuizTableStorageConnectionString`).
- Deploy `CSharpQuizFunc` to Azure using Visual Studio or the Azure CLI.

## Features
- Blazor WebAssembly SPA for C# quizzes
- Instant feedback and scoring
- Result submission to Azure Table Storage
- Unit-tested quiz logic

## License
This project is licensed under the MIT License.
