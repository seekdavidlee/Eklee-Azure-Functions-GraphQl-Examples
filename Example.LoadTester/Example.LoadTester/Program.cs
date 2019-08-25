using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Example.LoadTester
{
	class Program
	{
		static async Task Main(string[] args)
		{
			if (args.Length == 2)
			{
				Console.WriteLine("Running...");
				var fileParser = new LoadTestFileParser(args[0]);
				var loadTestRunner = new LoadTestRunner(await fileParser.GetLoadTestRunAsync());
				var result = await loadTestRunner.RunAsync(args[1], Console.WriteLine);
				var fileName = DateTime.Now.ToString("yyyyMMddHHmmss")+ ".txt";
				using (var sr = new StreamWriter(fileName))
					sr.Write(JsonConvert.SerializeObject(result));

				Console.WriteLine("Done. Press any key to continue.");
				Console.ReadKey();
			}
		}
	}
}
