using System;

namespace com.ez.engine.core.di
{
    public interface IResolver : IDisposable
    {
        Lifetime Lifetime { get; }
        object Resolve(Injector injector);
    }
}
