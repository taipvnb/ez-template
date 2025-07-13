namespace com.ez.engine.loots.core
{
	public class NullLoot : ILoot
	{
		private static NullLoot _instance;

		public static NullLoot Instance => _instance ??= new NullLoot();

		public string LootType => string.Empty;

		public LootParams ToLootParams()
		{
			return new LootParams(LootType, string.Empty, ';');
		}

		public ILoot Clone()
		{
			return Instance;
		}
	}
}
