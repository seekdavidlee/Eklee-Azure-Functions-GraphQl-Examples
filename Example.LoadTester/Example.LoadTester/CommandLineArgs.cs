using CommandLine;

namespace Example.LoadTester
{
	public class CommandLineArgs
	{
		[Option('r', "run-file-path", Required = true, HelpText = "Location of the run file.")]
		public string RunFilePath { get; set; }

		[Option('g', "graphql-url", Required = true, HelpText = "Url to the GraphQL endpoint.")]
		public string GraphQLUrl { get; set; }
	}
}
