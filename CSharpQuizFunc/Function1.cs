using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
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
				// Cosmos DB connection settings (use environment variables or configuration in production)
				string endpointUri = "https://csharpquiz.documents.azure.com:443/";
				string primaryKey = Environment.GetEnvironmentVariable("CosmosPrimaryKey");
				string databaseId = "quiz";
				string containerId = "quiz";


				CosmosClient cosmosClient = new CosmosClient(endpointUri, primaryKey);
				Container container = cosmosClient.GetContainer(databaseId, containerId);
				string id = container.Database.Id;
				var item = new
				{
					id = Guid.NewGuid().ToString(),
					Id = Guid.NewGuid().ToString(),
					Content = requestBody,
					timestamp = DateTime.UtcNow
				};

				await container.CreateItemAsync(item, new PartitionKey(item.Id));
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error: {ex.Message}");
			}

				string responseMessage = "Success";

			return new OkObjectResult(responseMessage);
		}
	}
}
