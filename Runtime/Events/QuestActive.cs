using com.ez.engine.message_bus;

namespace com.ez.engine.quest.core
{
    public struct QuestActive : IMessage
    {
        public string QuestId { get; }

        public QuestActive(string questId)
        {
            QuestId = questId;
        }
    }
}