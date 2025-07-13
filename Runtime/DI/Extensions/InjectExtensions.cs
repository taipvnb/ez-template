using System.Runtime.CompilerServices;

namespace com.ez.engine.core.di
{
    public static class InjectExtensions
    {
        private static readonly ConditionalWeakTable<Injector, DebugProperties> _containerDebugProperties = new();

        public static DebugProperties GetDebugProperties(this Injector container)
        {
            return _containerDebugProperties.GetOrCreateValue(container);
        }
    }
}
