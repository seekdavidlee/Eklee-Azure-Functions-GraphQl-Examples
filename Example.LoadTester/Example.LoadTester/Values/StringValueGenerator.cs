using System;

namespace Example.LoadTester.Values
{
	public class StringValueGenerator : IValueGenerator
	{
		private const string ID = "rand_string";
		private readonly Random _rand = new Random();

		public bool TryParse(ValueParser valueParser, out string value)
		{
			if (valueParser.Id == ID)
			{
				var min = Convert.ToInt32(valueParser.Min);
				var max = Convert.ToInt32(valueParser.Max);
				var length = _rand.Next(min, max);

				string str = Guid.NewGuid().ToString("N");
				while (true)
				{
					if (str.Length == length)
					{
						break;
					}

					if (str.Length> length)
					{
						str = str.Substring(0, length);
						break;
					}

					str += Guid.NewGuid().ToString("N");
				}

				value = $"\"{str}\"";
				return true;
			}
			value = "";
			return false;
		}
	}
}
