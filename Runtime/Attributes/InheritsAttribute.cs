using System;

namespace com.ez.engine.utils.type_references
{
	[AttributeUsage(AttributeTargets.Field)]
	public class InheritsAttribute : TypeOptionsAttribute
	{
		public Type BaseType { get; }

		public InheritsAttribute(Type baseType)
		{
			BaseType = baseType;
		}
	}
}
