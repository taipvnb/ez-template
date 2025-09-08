using com.ez.engine.message_bus;

namespace com.ez.engine.quest.core
{
    public struct QuestCompleted : IMessage
    {
        public string QuestId { get; }

        public QuestCompleted(string questId)
        {
            QuestId = questId;
        }
    }
}