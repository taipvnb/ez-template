using System.Collections.Generic;
using UnityEngine;

namespace com.ez.engine.quest.core
{
	public class QuestFactory
	{
		public Quest CreateQuest(string id, QuestStateType stateType, bool receiveReward,
			List<QuestConditionConfig> conditionConfigs,
			List<QuestGoalConfig> goalConfigs,
			List<QuestRewardConfig> rewardConfigs)
		{
			var quest = new GameObject($"Quest_{id}").AddComponent<Quest>();
			quest.SetId(id);
			quest.SetStateType(stateType);
			quest.SetReceiveReward(receiveReward);

			var configField = typeof(Quest).GetField("_conditionConfigs",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			configField?.SetValue(quest, conditionConfigs);

			configField = typeof(Quest).GetField("_goalConfigs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			configField?.SetValue(quest, goalConfigs);

			configField = typeof(Quest).GetField("_rewardConfigs",
				System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			configField?.SetValue(quest, rewardConfigs);

			return quest;
		}
	}
}
