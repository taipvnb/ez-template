namespace com.ez.engine.foundation
{
	public class LongOperator : IOperator<long>
	{
		public long MinValue => long.MinValue;

		public long MaxValue => long.MaxValue;

		public long Add(long a, long b) => a + b;

		public long Subtract(long a, long b) => a - b;

		public long Multiply(long a, long b) => a * b;

		public long Divide(long a, long b) => b == 0 ? a : a / b;
	}
}
