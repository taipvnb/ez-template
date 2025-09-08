using com.ez.engine.quest.core;
using UnityEngine;

namespace com.ez.engine.quest.core
{
	public abstract class QuestState : MonoBehaviour, IQuestState
	{
		public IQuestMachine Machine { get; set; }

		public virtual void Initialize() { }

		public virtual void Enter() { }

		public virtual void Execute() { }

		public virtual void Exit() { }
	}

	public abstract class QuestState<T> : QuestState where T : MonoBehaviour, IQuestMachine
	{
		public new T Machine => (T)base.Machine;
	}
}
