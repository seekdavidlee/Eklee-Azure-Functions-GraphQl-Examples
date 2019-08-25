using GraphQL.Client;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Example.LoadTester
{
	public class LoadTestRunner
	{
		private readonly LoadTestRun _loadTestRun;

		public LoadTestRunner(LoadTestRun loadTestRun)
		{
			_loadTestRun = loadTestRun;
		}

		public async Task<LoadTestRunStat> RunAsync(string url, Action<string> notify)
		{
			var loadTestRunStat = new LoadTestRunStat();
			var httpClient = new GraphQLClient(url);
			httpClient.Options.MediaType = new MediaTypeWithQualityHeaderValue("application/json");

			if (_loadTestRun.Parallel)
			{
				throw new NotSupportedException();
			}
			else
			{
				for (var i = 0; i < _loadTestRun.Run; i++)
				{
					if (_loadTestRun.Mutations != null)
					{
						notify($"Mutation Run {i}");
						foreach (var mutation in _loadTestRun.Mutations)
						{
							var run = new MutationRunner(httpClient, mutation);
							loadTestRunStat.RunsStat.Add(await run.RunAsync());
						}
					}
				}
			}

			loadTestRunStat.Summary = new LoadTestRunSummary
			{
				TotalPayloadSizeInMb = loadTestRunStat.RunsStat.Sum(x => x.PayloadSizeInKb) / 1000,
				AverageTimeInSeconds = loadTestRunStat.RunsStat.Average(x => (x.End - x.Start).TotalSeconds)
			};
			return loadTestRunStat;
		}
	}
}
