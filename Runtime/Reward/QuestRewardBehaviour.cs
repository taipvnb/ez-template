using UnityEngine;

namespace com.ez.engine.quest.core
{
	public abstract class QuestRewardBehaviour : MonoBehaviour, IQuestReward
	{
		public void GrantReward()
		{
			OnGrantReward();
		}

		protected abstract void OnGrantReward();
	}
}
