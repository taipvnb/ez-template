using System;

namespace com.ez.engine.quest.core
{
	public interface IQuestMachine
	{
		void Initialize();

		bool IsCurrentState<T>(bool allowBaseType = false) where T : IQuestState;

		bool IsCurrentState(Type type);

		bool HasState<T>() where T : IQuestState;

		bool HasState(Type type);

		T GetState<T>() where T : IQuestState;

		IQuestState GetState(Type type);

		bool AddState(IQuestState state);

		bool AddState<T>(T state) where T : IQuestState;

		void RemoveState(Type type);

		void RemoveState<T>() where T : IQuestState;

		void ChangeState(Type type);

		void ChangeState<T>() where T : IQuestState;
	}
}
