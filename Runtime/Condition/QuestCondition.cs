using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.ez.engine.quest.core
{
    [Serializable]
    public class QuestCondition
    {
        [SerializeField] private AssetType _assetType;

        [SerializeField, ShowIf("_assetType", AssetType.MonoBehaviour)]
        private QuestConditionBehaviour _conditionBehaviour;

        [SerializeField, ShowIf("_assetType", AssetType.ScriptableObject)]
        private QuestConditionAsset _conditionAsset;

        public AssetType AssetType
        {
            get => _assetType;
            set => _assetType = value;
        }

        public QuestConditionBehaviour ConditionBehaviour
        {
            get => _conditionBehaviour;
            set => _conditionBehaviour = value;
        }

        public QuestConditionAsset ConditionAsset
        {
            get => _conditionAsset;
            set => _conditionAsset = value;
        }

        public IQuestCondition GetCondition()
        {
            return _assetType switch
            {
                AssetType.MonoBehaviour => _conditionBehaviour,
                AssetType.ScriptableObject => Object.Instantiate(_conditionAsset),
                _ => null
            };
        }
    }
}