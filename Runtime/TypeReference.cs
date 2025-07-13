using System;

namespace com.ez.engine.utils.type_references
{
	[Serializable]
	public class TypeReference
	{
		public string AssemblyQualifiedName;

		[NonSerialized] private Type _cachedType;

		public Type Type
		{
			get
			{
				if (_cachedType == null && !string.IsNullOrEmpty(AssemblyQualifiedName))
				{
					_cachedType = Type.GetType(AssemblyQualifiedName);
				}

				return _cachedType;
			}
			set
			{
				_cachedType = value;
				AssemblyQualifiedName = value?.AssemblyQualifiedName;
			}
		}

		public override string ToString()
		{
			return Type?.Name ?? "None";
		}
	}
}
