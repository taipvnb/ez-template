using System;
using System.Linq;

namespace com.ez.engine.utils.type_references
{
	[AttributeUsage(AttributeTargets.Field)]
	public class TypeOptionsAttribute : Attribute
	{
		public bool ShowNoneElement { get; set; } = true;
		public bool ShortName { get; set; }
		public bool SerializableOnly { get; set; }
		public bool AllowInternal { get; set; }
		public Type[] IncludeTypes { get; set; }
		public Type[] ExcludeTypes { get; set; }
		public Grouping Grouping { get; set; } = Grouping.None;
		public bool ShowAllTypes { get; set; }

		public bool MatchesRequirements(Type type)
		{
			if (type == null)
			{
				return false;
			}

			if (SerializableOnly && !type.IsSerializable)
			{
				return false;
			}

			if (!AllowInternal && type.IsNotPublic)
			{
				return false;
			}

			if (ExcludeTypes != null && ExcludeTypes.Contains(type))
			{
				return false;
			}

			return true;
		}
	}
}
