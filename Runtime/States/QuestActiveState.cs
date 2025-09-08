namespace com.ez.engine.quest.core
{
	public class QuestActiveState : QuestState<Quest>
	{
		public override void Enter()
		{
			base.Enter();
			Machine.SetStateType(QuestStateType.Active);
			foreach (var goal in Machine.Goals)
			{
				goal.OnSuccess += OnSuccessHandler;
				goal.Activate();
			}
		}

		public override void Exit()
		{
			base.Exit();
			foreach (var goal in Machine.Goals)
			{
				goal.OnSuccess -= OnSuccessHandler;
				goal.Deactivate();
			}
		}

		private void OnSuccessHandler()
		{
			var complete = true;
			foreach (var goal in Machine.Goals)
			{
				if (goal.Success)
				{
					continue;
				}

				complete = false;
				break;
			}

			if (complete)
			{
				Machine.ChangeState<QuestCompleteState>();
			}
		}
	}
}
