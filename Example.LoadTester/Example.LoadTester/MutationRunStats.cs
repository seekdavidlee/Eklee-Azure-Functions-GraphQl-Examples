using GraphQL.Common.Response;
using System;

namespace Example.LoadTester
{
	public class MutationRunStats
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public double PayloadSizeInKb { get; set; }
		public GraphQLError[] Errors { get; set; }
	}
}
