using System.Collections.Generic;

namespace com.ez.engine.core.di
{
    public sealed class ResolverDebugProperties
    {
        public int Resolutions;
        public List<(object, List<CallSite>)> Instances { get; } = new();
        public List<CallSite> BindingCallsite { get; } = new();
    }
}
