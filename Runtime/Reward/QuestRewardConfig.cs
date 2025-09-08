using System;

namespace com.ez.engine.quest.core
{
	[Serializable]
	public abstract class QuestRewardConfig
	{
		public abstract IQuestReward CreateReward();
	}
}
