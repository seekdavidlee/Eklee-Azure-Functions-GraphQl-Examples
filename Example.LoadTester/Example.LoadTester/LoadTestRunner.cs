using GraphQL.Client;
using System;
using System.Collections.Generic;
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
			var loadTestRunStat = new LoadTestRunStat { GraphQLUrl = url };
			var httpClient = new GraphQLClient(url);
			httpClient.Options.MediaType = new MediaTypeWithQualityHeaderValue("application/json");

			if (_loadTestRun.Parallel != null &&
				_loadTestRun.Parallel.Threads > 0 &&
				_loadTestRun.Run > _loadTestRun.Parallel.Threads)
			{
				var tasks = new List<Task>();

				var remainder = _loadTestRun.Run % _loadTestRun.Parallel.Threads;
				var runsPerThread = (_loadTestRun.Run - remainder) / _loadTestRun.Parallel.Threads;

				int lastThreadIndex = _loadTestRun.Parallel.Threads - 1;
				for (int threadCounter = 0; threadCounter < _loadTestRun.Parallel.Threads; threadCounter++)
				{
					tasks.Add(RunBatch(threadCounter == lastThreadIndex ? runsPerThread + remainder : runsPerThread,
						loadTestRunStat, httpClient, notify, threadCounter));
				}

				await Task.WhenAll(tasks);
			}
			else
			{
				await RunBatch(_loadTestRun.Run, loadTestRunStat, httpClient, notify);
			}

			loadTestRunStat.Summary = new LoadTestRunSummary
			{
				TotalPayloadSizeInMb = loadTestRunStat.RunsStat.Sum(x => x.PayloadSizeInKb) / 1000,
				AverageTimeInSeconds = loadTestRunStat.RunsStat.Average(x => (x.End - x.Start).TotalSeconds),
				TotalTimeMinutes = loadTestRunStat.RunsStat.Sum(x => (x.End - x.Start).TotalMinutes),
			};
			return loadTestRunStat;
		}

		private const string THREAD_ZERO = "Thread 0";

		private async Task RunBatch(int count, LoadTestRunStat loadTestRunStat, GraphQLClient httpClient, Action<string> notify,
			int? threadCounter = null)
		{
			string appendNotify = threadCounter.HasValue ? $"Thread {threadCounter.Value}" : THREAD_ZERO;

			for (var i = 0; i < count; i++)
			{
				if (_loadTestRun.Mutations != null)
				{
					notify($"{appendNotify} Mutation Run {i}");
					foreach (var mutation in _loadTestRun.Mutations)
					{
						var run = new MutationRunner(httpClient, mutation);
						loadTestRunStat.RunsStat.Add(await run.RunAsync());
					}
				}
			}
		}
	}
}
