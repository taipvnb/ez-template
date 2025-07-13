using com.ez.engine.resources.core;

namespace com.ez.engine.loots.core
{
	public interface ILootHandler
	{
		string LootType { get; }

		void Cache(ILoot loot);

		void Handle(ILoot loot, ResourceAddress address);
	}
}
