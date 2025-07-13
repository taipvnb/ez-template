namespace com.ez.engine.loots.core
{
	public interface ILootCreator
	{
		string LootType { get; }

		ILoot Create(LootParams lootParams);
	}
}
