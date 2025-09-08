using com.ez.engine.core.di;
using com.ez.engine.services.message_bus;

namespace com.ez.engine.quest.core
{
	public class QuestInActiveState : QuestState<Quest>
	{
		[Inject] private readonly IMessageBusService _messageBusService;

		public override void Enter()
		{
			base.Enter();
			Machine.SetStateType(QuestStateType.InActive);
			foreach (var condition in Machine.Conditions)
			{
				condition.OnPassed += OnPassed;
				condition.Activate();
			}
		}

		public override void Exit()
		{
			base.Exit();
			foreach (var condition in Machine.Conditions)
			{
				condition.OnPassed -= OnPassed;
				condition.Deactivate();
			}
		}

		private void OnPassed()
		{
			var passed = true;
			foreach (var condition in Machine.Conditions)
			{
				if (condition.Passed)
				{
					continue;
				}

				passed = false;
				break;
			}

			if (passed)
			{
				Machine.ChangeState<QuestActiveState>();
				_messageBusService?.Dispatch(new QuestActive(Machine.Id));
			}
		}
	}
}
