using System;

namespace com.ez.engine.save.core
{
	public abstract class BaseRuntimeModel : IRuntimeModel, IDisposable
	{
		public abstract int Version { get; }

		public virtual int SaveThreshold { get; } = 1;

		public virtual void Dispose() { }
	}
}
