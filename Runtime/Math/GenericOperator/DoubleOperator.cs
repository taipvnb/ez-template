namespace com.ez.engine.foundation
{
	public class DoubleOperator : IOperator<double>
	{
		public double MinValue => double.MinValue;

		public double MaxValue => double.MaxValue;

		public double Add(double a, double b) => a + b;

		public double Subtract(double a, double b) => a - b;

		public double Multiply(double a, double b) => a * b;

		public double Divide(double a, double b) => b == 0 ? a : a / b;
	}
}
