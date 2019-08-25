namespace Example.LoadTester.Values
{
	public class ValueParser
	{
		public ValueParser(string expression)
		{
			var startOf = expression.IndexOf("(", 1) - 1;
			Id = expression.Substring(1, startOf);

			startOf = Id.Length + 2;
			var args = expression.Substring(startOf, expression.IndexOf(")") - startOf).Split(',');
			if (args.Length == 2)
			{
				Min = args[0];
				Max = args[1];
			}
		}

		public string Id { get; }

		public string Min { get; }

		public string Max { get; }
	}
}
