using System.Collections.Generic;

namespace Example.LoadTester
{
	public class MutationInput
	{
		public string Type { get; set; }
		public int? BatchCount { get; set; }
		public Dictionary<string, object> Value { get; set; }
	}
}
