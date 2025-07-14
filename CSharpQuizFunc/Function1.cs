using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using static CSharpQuizFunc.Function1;
using static System.Net.WebRequestMethods;

namespace CSharpQuizFunc
{
	public static class Function1
	{
		[FunctionName("Function1")]
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

	public class MyEntity : ITableEntity
	{
		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public string Data { get; set; }
		public ETag ETag { get; set; } = ETag.All;
		public DateTimeOffset? Timestamp { get; set; }
	}
}
