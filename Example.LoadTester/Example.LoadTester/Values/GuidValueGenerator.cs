using System;

namespace Example.LoadTester.Values
{
	public class GuidValueGenerator : IValueGenerator
	{
		private const string ID = "new_guid";
		public bool TryParse(ValueParser valueParser, out string value)
		{
			if (valueParser.Id == ID)
			{
				value = $"\"{Guid.NewGuid().ToString("N")}\"";
				return true;
			}
			value = "";
			return false;
		}
	}
}
