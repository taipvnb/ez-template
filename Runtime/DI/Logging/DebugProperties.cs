using System.Collections.Generic;

namespace com.ez.engine.core.di
{
    public sealed class DebugProperties
    {
        public List<CallSite> BuildCallsite { get; } = new();
    }
}
