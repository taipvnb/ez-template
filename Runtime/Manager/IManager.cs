using System;
using Cysharp.Threading.Tasks;

namespace com.ez.engine.core.manager
{
	public interface IManager : IDisposable
	{
		int Priority { get; }

		bool IsInitialized { get; }

		UniTask Initialize();
	}
}
