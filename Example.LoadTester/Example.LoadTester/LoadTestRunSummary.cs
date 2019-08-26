namespace Example.LoadTester
{
	public class LoadTestRunSummary
	{
		public double TotalPayloadSizeInMb { get; set; }
		public double AverageTimeInSeconds { get; internal set; }
		public double TotalTimeMinutes { get; set; }
	}
}
