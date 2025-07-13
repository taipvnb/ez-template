namespace com.ez.engine.foundation
{
	public class FloatOperator : IOperator<float>
	{
		public float MinValue => float.MinValue;

		public float MaxValue => float.MaxValue;

		public float Add(float a, float b) => a + b;

		public float Subtract(float a, float b) => a - b;

		public float Multiply(float a, float b) => a * b;

		public float Divide(float a, float b) => b == 0 ? a : a / b;
	}
}
