using System.Collections.Generic;
using System.Linq;
using com.ez.engine.core.di;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.ez.engine.quest.core
{
	public class Quest : QuestMachine
	{
		[SerializeField] private string _id;
		[SerializeField] private QuestStateType _stateType;
		[SerializeField] private bool _receiveReward;
		[SerializeField] private List<QuestCondition> _conditions;
		[SerializeField] private List<QuestGoal> _goals;
		[SerializeField] private List<QuestReward> _rewards;

		[Inject] private readonly IInjector _injector;

		public string Id => _id;

		public string Name { get; private set; }

		public QuestStateType StateType => _stateType;

		public bool Claimable => _stateType == QuestStateType.Complete && !_receiveReward;

		public bool ReceiveReward => _receiveReward;

		public float GoalProgress => Goals.Count > 0 ? Goals.Average(goal => goal.Progress) : 0f;

		[ShowInInspector] public List<IQuestCondition> Conditions { get; private set; } = new();

		[ShowInInspector] public List<IQuestGoal> Goals { get; private set; } = new();

		[ShowInInspector] public List<IQuestReward> Rewards { get; private set; } = new();

		public override void Initialize()
		{
			base.Initialize();
			_receiveReward = false;

			foreach (var state in States)
			{
				_injector?.Resolve(state);
			}

			foreach (var condition in _conditions)
			{
				var conditionImpl = condition.GetCondition();
				_injector?.Resolve(conditionImpl);
				Conditions.Add(conditionImpl);
			}

			foreach (var goal in _goals)
			{
				var goalImpl = goal.GetGoal();
				_injector?.Resolve(goalImpl);
				Goals.Add(goalImpl);
			}

			foreach (var reward in _rewards)
			{
				var rewardImpl = reward.GetReward();
				_injector?.Resolve(rewardImpl);
				Rewards.Add(rewardImpl);
			}

			ChangeState<QuestInActiveState>();
		}

		public List<GoalValueInfo> GetGoalValues()
		{
			if (Goals == null || Goals.Count == 0)
			{
				return new List<GoalValueInfo>();
			}

			return Goals.Select(goal => new GoalValueInfo(
				goal.Id,
				goal.CurrentValueAsObject,
				goal.RequiredValueAsObject,
				goal.Progress,
				goal.ValueType
			)).ToList();
		}

		public void SetId(string id)
		{
			_id = id;
		}

		public void SetName(string cname)
		{
			Name = cname;
		}

		public void SetStateType(QuestStateType stateType)
		{
			_stateType = stateType;
		}

		public void SetReceiveReward(bool receiveReward)
		{
			_receiveReward = receiveReward;
		}

		public void AddCondition(IQuestCondition condition)
		{
			if (Conditions.Contains(condition))
			{
				return;
			}

			_injector?.Resolve(condition);
			Conditions.Add(condition);
		}

		public void AddGoal(IQuestGoal goal)
		{
			if (Goals.Contains(goal))
			{
				return;
			}

			_injector?.Resolve(goal);
			Goals.Add(goal);
		}

		public void AddReward(IQuestReward reward)
		{
			if (Rewards.Contains(reward))
			{
				return;
			}

			_injector?.Resolve(reward);
			Rewards.Add(reward);
		}

		public void ClaimReward()
		{
			_receiveReward = true;
			foreach (var reward in _rewards)
			{
				var rewardImpl = reward.GetReward();
				rewardImpl.GrantReward();
			}

			ChangeState<QuestDoneState>();
		}
	}
}
