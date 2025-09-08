using System;

namespace com.ez.engine.quest.core
{
    [Serializable]
    public abstract class QuestConditionConfig
    {
        public abstract IQuestCondition CreateCondition();
    }
}