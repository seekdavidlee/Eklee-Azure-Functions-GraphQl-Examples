using System.Collections.Generic;

namespace Example.LoadTester
{
	public class LoadTestRun
	{
		public int Run { get; set; }
		public bool Parallel { get; set; }
		public List<Mutation> Mutations { get; set; }
	}
}
