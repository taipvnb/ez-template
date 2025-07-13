using System.Collections.Generic;
using com.ez.engine.command_bus;
using com.ez.engine.loots.core;
using com.ez.engine.resources.core;

namespace com.ez.engine.manager.loot
{
	public class ClaimRewardCommand : ICommand
	{
		public List<ILoot> Loots { get; }

		public ResourceAddress Address { get; }

		public ClaimRewardCommand(ILoot loot, ResourceAddress address)
		{
			Loots = new List<ILoot> { loot };
			Address = address;
		}

		public ClaimRewardCommand(List<ILoot> loots, ResourceAddress address)
		{
			Loots = loots;
			Address = address;
		}
	}
}
