using System;

namespace com.ez.engine.quest.core
{
	[Serializable]
	public abstract class QuestGoalConfig
	{
		public abstract IQuestGoal CreateGoal();
	}
}
