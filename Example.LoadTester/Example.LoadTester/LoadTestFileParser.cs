using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Example.LoadTester
{
	public class LoadTestFileParser
	{
		private readonly string _path;

		public LoadTestFileParser(string path)
		{
			_path = path;
		}

		public async Task<LoadTestRun> GetLoadTestRunAsync()
		{
			using (var sr = new StreamReader(_path))
			{
				return JsonConvert.DeserializeObject<LoadTestRun>(await sr.ReadToEndAsync());
			}
		}
	}
}
