using System;

namespace com.ez.engine.resources.core
{
	public interface IResource
	{
		string Id { get; }
	}

	public interface IResource<T> : IResource
	{
		event Action<T> OnAmountChanged;

		event Action<T> OnValueChanged;

		T Amount { get; }

		void SetAmount(T amount);

		void Add(T amount);

		void Subtract(T amount);

		void Multiply(T amount);

		void Divide(T amount);
	}
}
