using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Example.Storage.Core;
using Eklee.Azure.Functions.Http;
using Eklee.Azure.Functions.GraphQl;

namespace Example.Storage
{
	public static class GraphQLFunction
	{
		[ExecutionContextDependencyInjection(typeof(FunctionModule))]
		[FunctionName("GraphQLFunction")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "teststorage")] HttpRequest req,
			ILogger log,
			ExecutionContext executionContext)
		{
			log.LogInformation("Executed teststorage.");
			return await executionContext.ProcessGraphQlRequest(req);
		}
	}
}
