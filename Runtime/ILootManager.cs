using System.Collections.Generic;
using com.ez.engine.core.manager;
using com.ez.engine.loots.core;
using com.ez.engine.resources.core;

namespace com.ez.engine.manager.loot
{
	public interface ILootManager : IManager
	{
		void Loot(LootParams lootParams, ResourceAddress address);

		void Loot(List<LootParams> lootParams, ResourceAddress address);

		void Loot(ILoot loot, ResourceAddress address);

		void Loot(List<ILoot> loots, ResourceAddress address);

		ILoot CreateLoot(LootParams lootParams);

		List<ILoot> CreateLoots(List<LootParams> lootParams);

		THandler GetHandler<THandler>() where THandler : ILootHandler;

		bool Handle(ILoot loot, ResourceAddress address);

		bool Handle(List<ILoot> loots, ResourceAddress address);
	}
}
