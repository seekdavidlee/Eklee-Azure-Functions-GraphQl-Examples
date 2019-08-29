using System;
using System.Collections.Generic;

namespace Example.LoadTester
{
	public class LoadTestRunStat
	{
		public LoadTestRunStat()
		{
			RunsStat = new List<MutationRunStats>();
			Executed = DateTime.UtcNow;
		}

		public string GraphQLUrl { get; set; }

		public DateTime Executed { get; }
		public List<MutationRunStats> RunsStat { get; }

		public LoadTestRunSummary Summary { get; set; }
	}
}
