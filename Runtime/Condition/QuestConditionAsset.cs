using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.ez.engine.quest.core
{
    [Serializable]
    [CreateAssetMenu(menuName = "Unimob/Quest System/QuestConditionAsset")]
    public abstract class QuestConditionAsset : ScriptableObject, IQuestCondition
    {
        [ShowInInspector, ReadOnly] public bool Passed { get; protected set; }

        public Action OnPassed { get; set; }

        public void Activate()
        {
            OnActivate();
        }

        public void Deactivate()
        {
            OnDeactivate();
        }

        protected abstract void OnActivate();
        protected abstract void OnDeactivate();
    }
}