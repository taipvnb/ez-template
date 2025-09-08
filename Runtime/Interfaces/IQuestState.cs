namespace com.ez.engine.quest.core
{
	public interface IQuestState
	{
		IQuestMachine Machine { get; set; }

		void Initialize();

		void Enter();

		void Execute();

		void Exit();
	}
}
