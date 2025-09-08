using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.ez.engine.quest.core
{
	public abstract class QuestMachine : MonoBehaviour, IQuestMachine
	{
		public IQuestState CurrentState { get; private set; }

		[ShowInInspector] private readonly Dictionary<Type, IQuestState> _states = new();

		public IEnumerable<Type> StateKeys => _states.Keys;

		public IEnumerable<IQuestState> States => _states.Values;

		public virtual void Initialize()
		{
			var questStates = GetComponents<IQuestState>();
			foreach (var state in questStates)
			{
				AddState(state);
			}

			foreach (var state in _states)
			{
				state.Value.Initialize();
			}
		}

		public bool IsCurrentState<T>(bool allowBaseType = false) where T : IQuestState
		{
			return allowBaseType ? CurrentState.GetType().BaseType == typeof(T) : CurrentState.GetType() == typeof(T);
		}

		public bool IsCurrentState(Type type)
		{
			return CurrentState.GetType() == type;
		}

		public bool HasState<T>() where T : IQuestState
		{
			return _states.ContainsKey(typeof(T));
		}

		public bool HasState(Type type)
		{
			return _states.ContainsKey(type);
		}

		public T GetState<T>() where T : IQuestState
		{
			return (T)_states[typeof(T)];
		}

		public IQuestState GetState(Type type)
		{
			return _states[type];
		}

		public bool AddState<T>(T state) where T : IQuestState
		{
			var type = typeof(T);
			if (HasState(type))
			{
				return false;
			}

			state.Machine = this;
			_states.Add(type, state);
			return true;
		}

		public bool AddState(IQuestState state)
		{
			var type = state.GetType();
			if (HasState(type))
			{
				return false;
			}

			state.Machine = this;
			_states.Add(type, state);
			return true;
		}

		public void RemoveState<T>() where T : IQuestState
		{
			_states.Remove(typeof(T));
		}

		public void RemoveState(Type type)
		{
			_states.Remove(type);
		}

		public virtual void ChangeState(Type type)
		{
			if (!HasState(type))
			{
				Debug.LogError("Missing state " + type.Name + " on machine " + gameObject.name);
				return;
			}

			CurrentState?.Exit();
			CurrentState = _states[type];
			CurrentState.Enter();
		}

		public virtual void ChangeState<T>() where T : IQuestState
		{
			ChangeState(typeof(T));
		}
	}
}
