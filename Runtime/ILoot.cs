namespace com.ez.engine.loots.core
{
	public interface ILoot
	{
		string LootType { get; }

		ILoot Clone();

		LootParams ToLootParams();
	}

	public interface ILoot<out T> : ILoot
	{
		T Amount { get; }
	}
}
