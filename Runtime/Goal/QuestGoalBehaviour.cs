using System;
using UnityEngine;

namespace com.ez.engine.quest.core
{
	public abstract class QuestGoalBehaviour<T> : MonoBehaviour, IQuestGoal<T>
	{
		[SerializeField] private int _id;

		public bool Success { get; protected set; }

		public Action OnSuccess { get; set; }

		public int Id => _id;

		public abstract T CurrentValue { get; set; }

		public abstract T RequiredValue { get; set; }

		public Type ValueType => typeof(T);

		public object CurrentValueAsObject => CurrentValue;
		public object RequiredValueAsObject => RequiredValue;
		public abstract float Progress { get; }

		public void Activate()
		{
			OnActivate();
		}

		public void Deactivate()
		{
			OnDeactivate();
		}

		protected abstract void OnActivate();
		protected abstract void OnDeactivate();
	}
}
