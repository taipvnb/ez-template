using com.ez.engine.core.di;
using com.ez.engine.services.message_bus;

namespace com.ez.engine.quest.core
{
    public class QuestCompleteState : QuestState<Quest>
    {
        [Inject] private readonly IMessageBusService _messageBusService;

        public override void Enter()
        {
            base.Enter();
            Machine.SetStateType(QuestStateType.Complete);
            _messageBusService?.Dispatch(new QuestCompleted(Machine.Id));
        }
    }
}