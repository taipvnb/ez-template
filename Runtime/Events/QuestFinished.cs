using com.ez.engine.message_bus;

namespace com.ez.engine.quest.core
{
    public struct QuestFinished : IMessage
    {
        public string QuestId { get; }

        public QuestFinished(string questId)
        {
            QuestId = questId;
        }
    }
}