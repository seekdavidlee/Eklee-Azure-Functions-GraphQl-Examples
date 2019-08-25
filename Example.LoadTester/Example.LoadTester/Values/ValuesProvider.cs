using System.Collections.Generic;

namespace Example.LoadTester.Values
{
	public class ValuesProvider
	{
		private const string NOT_IMPLEMENTED = "\"NOT_IMPLEMENTED\"";
		private readonly List<IValueGenerator> _valueGenerators = new List<IValueGenerator> {
			new GuidValueGenerator(), new StringValueGenerator(), new MoneyValueGenerator()
		};

		public string Generate(string expression)
		{
			var valueParser = new ValueParser(expression);
			for (var i = 0; i < _valueGenerators.Count; i++)
			{
				string line;
				if (_valueGenerators[i].TryParse(valueParser, out line))
				{
					return line;
				}
			}

			return NOT_IMPLEMENTED;
		}
	}
}
