using System.Runtime.CompilerServices;

namespace com.ez.engine.core.di
{
	public static class ResolverExtensions
	{
		private static readonly ConditionalWeakTable<IResolver, ResolverDebugProperties> _registry = new();

		public static ResolverDebugProperties GetDebugProperties(this IResolver resolver)
		{
			return _registry.GetOrCreateValue(resolver);
		}
	}
}
