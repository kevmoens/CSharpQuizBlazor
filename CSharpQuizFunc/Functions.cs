using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CSharpQuizFunc
{
	public static class Results
	{
		[FunctionName("Results")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("C# HTTP trigger function processed a request.");

			//string name = req.Query["name"];

			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			//dynamic data = JsonConvert.DeserializeObject(requestBody);
			//name = name ?? data?.name;
			try
			{

				//enable below code for storage access with entra id auth            
				string connectionString =  Environment.GetEnvironmentVariable("QuizTableStorageConnectionString");
				string tableName = "quiz";

				// Create a TableClient
				TableClient tableClient = new TableClient(connectionString, tableName);


				// Create an entity
				var entity = new MyEntity
				{
					PartitionKey = "Quiz",
					RowKey = Guid.NewGuid().ToString(),
					Data = requestBody
				};

				// Add the entity to the table
				await tableClient.AddEntityAsync(entity);

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}

			string responseMessage = "Success";

			return new OkObjectResult(responseMessage);
		}
	}

	/// <summary>
	/// https://csharpquizfunc20250713230151.azurewebsites.net/api/GetTestNames
	/// </summary>
	public static class GetTestNames
	{
		[FunctionName("GetTestNames")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("GetTestNames HTTP trigger function processed a request.");

			try
			{

				//enable below code for storage access with entra id auth            
				string connectionString = Environment.GetEnvironmentVariable("QuizTableStorageConnectionString");
				string tableName = "test";

				// Create a TableClient
				TableClient tableClient = new TableClient(connectionString, tableName);
				AsyncPageable<MyEntity> entities = tableClient.QueryAsync<MyEntity>();
				List<string> tests = [];
				await foreach (MyEntity entry in entities)
				{
					Console.WriteLine($"PartitionKey: {entry.PartitionKey}, RowKey: {entry.RowKey}, Data: {entry.Data}");
					tests.Add(entry.PartitionKey);
				}

				return new OkObjectResult(tests);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"GetTestNames Error: {ex.Message}");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

		}
	}


	/// <summary>
	/// for C#
	/// https://csharpquizfunc20250713230151.azurewebsites.net/api/GetestQuestions?name=C%23
	/// for SQL
	/// https://csharpquizfunc20250713230151.azurewebsites.net/api/GetestQuestions?name=SQL
	/// </summary>
	public static class GetTestQuestions
	{
		[FunctionName("GetTestQuestions")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
			ILogger log)
		{
			log.LogInformation("GetTestQuestions HTTP trigger function processed a request.");

			try
			{					
				string connectionString = Environment.GetEnvironmentVariable("QuizTableStorageConnectionString");
				string tableName = "test";
				TableClient tableClient = new TableClient(connectionString, tableName);

				string name = req.Query["name"];
				AsyncPageable<MyEntity> entities = tableClient.QueryAsync<MyEntity>(entity => entity.PartitionKey == name);
				MyEntity? quizEntity = null;
				await foreach (MyEntity entity in entities)
				{
					quizEntity = entity;
					break; // Assuming only one entity per test name
				}
				if (quizEntity == null)
				{
					return new NotFoundObjectResult($"Test with name '{name}' not found.");
				}
				Quiz quiz = JsonConvert.DeserializeObject<Quiz>(quizEntity.Data);
				
				return new OkObjectResult(quiz);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"GetTestQuestions Error: {ex.Message}");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

		}
	}

	//Azure Table Storage entity class
	public class MyEntity : ITableEntity
	{
		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public string Data { get; set; }
		public ETag ETag { get; set; } = ETag.All;
		public DateTimeOffset? Timestamp { get; set; }
	}

	public class Quiz
	{
		public List<Question> Questions { get; set; } = [];
	}
	public class Question
	{
		public string Text { get; set; }
		public List<string> Choices { get; set; }
		public int CorrectChoiceIndex { get; set; }
	}
}
