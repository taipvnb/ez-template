namespace com.ez.engine.foundation
{
	public interface IOperator<T>
	{
		T MinValue { get; }

		T MaxValue { get; }

		T Add(T a, T b);

		T Subtract(T a, T b);

		T Multiply(T a, T b);

		T Divide(T a, T b);
	}
}
