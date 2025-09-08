using System;
using UnityEngine;

namespace com.ez.engine.quest.core
{
	[Serializable]
	[CreateAssetMenu(menuName = "EzEngine/Quest System/QuestRewardAsset")]
	public abstract class QuestRewardAsset : ScriptableObject, IQuestReward
	{
		public void GrantReward()
		{
			OnGrantReward();
		}

		protected abstract void OnGrantReward();
	}
}
