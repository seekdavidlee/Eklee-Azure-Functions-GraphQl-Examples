﻿using CommandLine;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Example.LoadTester
{
	class Program
	{
		static async Task Main(string[] args)
		{
			await Parser.Default.ParseArguments<CommandLineArgs>(args)
				.MapResult(async cmdArgs =>
			{
				Console.WriteLine("Running...");

				var fileParser = new LoadTestFileParser(cmdArgs.RunFilePath);

				var loadTestRunner = new LoadTestRunner(await fileParser.GetLoadTestRunAsync());

				var result = await loadTestRunner.RunAsync(cmdArgs.GraphQLUrl, Console.WriteLine);

				var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
				using (var sr = new StreamWriter(fileName))
					sr.Write(JsonConvert.SerializeObject(result));
			}, errors => Task.FromResult(0));
		}
	}
}
