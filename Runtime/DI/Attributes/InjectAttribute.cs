using System;
using JetBrains.Annotations;

namespace com.ez.engine.core.di
{
	[MeansImplicitUse(ImplicitUseKindFlags.Assign)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
	public class InjectAttribute : Attribute { }
}
