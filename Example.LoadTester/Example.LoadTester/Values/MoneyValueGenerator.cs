using System;
using System.Collections.Generic;
using System.Text;

namespace Example.LoadTester.Values
{
	public class MoneyValueGenerator : IValueGenerator
	{
		private const string ID = "rand_money";
		private readonly Random _rand = new Random();

		public bool TryParse(ValueParser valueParser, out string value)
		{
			if (valueParser.Id == ID)
			{
				var min = Convert.ToDouble(valueParser.Min);
				var max = Convert.ToDouble(valueParser.Max);

				var money = _rand.NextDouble() * (max - min) + min;
				value = Math.Round(money, 2).ToString();
				return true;
			}
			value = "";
			return false;
		}
	}
}
