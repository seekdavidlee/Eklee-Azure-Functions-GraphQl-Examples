namespace Example.LoadTester.Values
{
	public interface IValueGenerator
	{
		bool TryParse(ValueParser valueParser, out string value);
	}
}
