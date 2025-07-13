namespace com.ez.engine.resources.core
{
	public class ResourceAddress
	{
		public string Source { get; }
		public string Location { get; }

		private ResourceAddress(string location, string source)
		{
			Source = source;
			Location = location;
		}

		public static ResourceAddress New(string location, string source)
		{
			return new ResourceAddress(location, source);
		}

		public static readonly ResourceAddress Null = new(string.Empty, string.Empty);
	}
}
