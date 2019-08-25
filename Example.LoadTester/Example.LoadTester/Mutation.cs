using System.Collections.Generic;

namespace Example.LoadTester
{
	public class Mutation
	{
		public string Name { get; set; }
		public List<MutationInput> Inputs { get; set; }
		public string Output { get; set; }
	}
}
