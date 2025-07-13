using System;
using System.Collections.Generic;

namespace com.ez.engine.foundation
{
	public static class OperatorMapper
	{
		private static readonly Dictionary<Type, object> Operators = new()
		{
			{ typeof(int), new IntOperator() },
			{ typeof(float), new FloatOperator() },
			{ typeof(double), new DoubleOperator() },
			{ typeof(long), new LongOperator() }
		};

		public static void AddOperator<T>(IOperator<T> @operator)
		{
			Operators.TryAdd(typeof(T), @operator);
		}

		public static IOperator<T> GetOperator<T>()
		{
			if (Operators.TryGetValue(typeof(T), out var @operator))
			{
				return (IOperator<T>)@operator;
			}

			throw new NotSupportedException($"No operator found for type {typeof(T)}");
		}
	}
}
