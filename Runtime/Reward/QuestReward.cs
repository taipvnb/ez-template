using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.ez.engine.quest.core
{
	[Serializable]
	public class QuestReward
	{
		[SerializeField] private AssetType _assetType;

		[SerializeField, ShowIf("_assetType", AssetType.MonoBehaviour)]
		private QuestRewardBehaviour _rewardBehaviour;

		[SerializeField, ShowIf("_assetType", AssetType.ScriptableObject)]
		private QuestRewardAsset _rewardAsset;

		public AssetType AssetType
		{
			get => _assetType;
			set => _assetType = value;
		}

		public QuestRewardBehaviour RewardBehaviour
		{
			get => _rewardBehaviour;
			set => _rewardBehaviour = value;
		}

		public QuestRewardAsset RewardAsset
		{
			get => _rewardAsset;
			set => _rewardAsset = value;
		}

		public IQuestReward GetReward()
		{
			return _assetType switch
			{
				AssetType.MonoBehaviour => _rewardBehaviour,
				AssetType.ScriptableObject => Object.Instantiate(_rewardAsset),
				_ => null
			};
		}
	}
}
