using System;
using UnityEngine;

namespace com.ez.engine.quest.core
{
    [Serializable]
    [CreateAssetMenu(menuName = "Unimob/Quest System/Condition/EmptyConditionAsset")]
    public class EmptyConditionAsset : QuestConditionAsset
    {
        protected override void OnActivate()
        {
            Passed = true;
            OnPassed?.Invoke();
        }

        protected override void OnDeactivate()
        {
        }
    }
}