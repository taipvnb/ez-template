using System;

namespace com.ez.engine.quest.core
{
	public interface IQuestCondition
	{
		bool Passed { get; }

		Action OnPassed { get; set; }

		void Activate();

		void Deactivate();
	}
}
