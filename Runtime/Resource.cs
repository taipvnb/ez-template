using System;
using com.ez.engine.foundation;

namespace com.ez.engine.resources.core
{
	[Serializable]
	public class Resource<T> : IResource<T> where T : IComparable
	{
		public string Id { get; }

		public event Action<T> OnAmountChanged;

		public event Action<T> OnValueChanged;

		public T Amount
		{
			get => _amount;
			private set
			{
				if (value.CompareTo(_minValue) < 0)
				{
					value = _minValue;
				}
				else if (value.CompareTo(_maxValue) > 0)
				{
					value = _maxValue;
				}

				OnAmountChanged?.Invoke(_operator.Subtract(value, _amount));

				_amount = value;

				OnValueChanged?.Invoke(_amount);
			}
		}

		private T _amount;
		private T _minValue;
		private T _maxValue;
		private IOperator<T> _operator;

		public Resource() { }

		public Resource(string id) : this()
		{
			Id = id;
			_operator = OperatorMapper.GetOperator<T>();
			_minValue = _operator.MinValue;
			_maxValue = _operator.MaxValue;
		}

		public Resource(string id, T amount) : this(id)
		{
			Id = id;
			Amount = amount;
		}

		public Resource(string id, T amount, T minValue, T maxValue) : this(id, amount)
		{
			_minValue = minValue;
			_maxValue = maxValue;
		}

		public void SetAmount(T amount)
		{
			Amount = amount;
		}

		public void SetMinValue(T minValue)
		{
			_minValue = minValue;
		}

		public void SetMaxValue(T maxValue)
		{
			_maxValue = maxValue;
		}

		public void Add(T amount)
		{
			Amount = _operator.Add(Amount, amount);
		}

		public void Subtract(T amount)
		{
			Amount = _operator.Subtract(Amount, amount);
		}

		public void Multiply(T amount)
		{
			Amount = _operator.Multiply(Amount, amount);
		}

		public void Divide(T amount)
		{
			Amount = _operator.Divide(Amount, amount);
		}
	}
}
