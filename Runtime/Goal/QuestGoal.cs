using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.ez.engine.quest.core
{
	[Serializable]
	public class QuestGoal
	{
		[SerializeField] private AssetType _assetType;

		[SerializeField, ShowIf("_assetType", AssetType.MonoBehaviour)]
		private MonoBehaviour _goalBehaviour;

		[SerializeField, ShowIf("_assetType", AssetType.ScriptableObject)]
		private ScriptableObject _goalAsset;

		public AssetType AssetType
		{
			get => _assetType;
			set => _assetType = value;
		}

		public MonoBehaviour GoalBehaviour
		{
			get => _goalBehaviour;
			set => _goalBehaviour = value;
		}

		public ScriptableObject GoalAsset
		{
			get => _goalAsset;
			set => _goalAsset = value;
		}

		public IQuestGoal GetGoal()
		{
			return _assetType switch
			{
				AssetType.MonoBehaviour => _goalBehaviour as IQuestGoal,
				AssetType.ScriptableObject => Object.Instantiate(_goalAsset) as IQuestGoal,
				_ => null
			};
		}
	}
}
