using System;

namespace com.ez.engine.quest.core
{
	[Serializable]
	public class GoalValueInfo
	{
		public int Id;
		public object CurrentValue;
		public object RequiredValue;
		public float Progress;
		public Type ValueType;

		public GoalValueInfo(int id, object currentValue, object requiredValue, float progress, Type valueType)
		{
			Id = id;
			CurrentValue = currentValue;
			RequiredValue = requiredValue;
			Progress = progress;
			ValueType = valueType;
		}

		public bool TryGetValues<T>(out T current, out T required)
		{
			current = default;
			required = default;

			if (ValueType != typeof(T))
			{
				return false;
			}

			try
			{
				current = (T)CurrentValue;
				required = (T)RequiredValue;
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
