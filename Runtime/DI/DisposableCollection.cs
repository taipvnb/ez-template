using System;
using System.Collections.Generic;

namespace com.ez.engine.core.di
{
	internal sealed class DisposableCollection : IDisposable
	{
		private readonly Stack<IDisposable> _stack = new();

		public void Add(IDisposable disposable)
		{
			_stack.Push(disposable);
		}

		public void TryAdd(object obj)
		{
			if (obj is IDisposable disposable)
			{
				_stack.Push(disposable);
			}
		}

		public void Remove(IDisposable disposable)
		{
			_stack.TryPop(out _);
		}

		public void Dispose()
		{
			while (_stack.TryPop(out var disposable))
			{
				disposable.Dispose();
			}
		}
	}
}
