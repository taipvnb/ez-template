using System;

namespace com.ez.engine.quest.core
{
	public interface IQuestGoal
	{
		int Id { get; }

		Type ValueType { get; }

		object CurrentValueAsObject { get; }

		object RequiredValueAsObject { get; }

		float Progress { get; }

		bool Success { get; }

		Action OnSuccess { get; set; }

		void Activate();

		void Deactivate();
	}

	public interface IQuestGoal<T> : IQuestGoal
	{
		T CurrentValue { get; set; }

		T RequiredValue { get; set; }
	}
}
