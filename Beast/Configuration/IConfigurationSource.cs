namespace Beast.Configuration
{
	public interface IConfigurationSource
	{
		bool Contains(string key);
		string GetValue(string key);
	}
}
