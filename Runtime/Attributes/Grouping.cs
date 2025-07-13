namespace com.ez.engine.utils.type_references
{
	public enum Grouping
	{
		/// <summary>
		/// No grouping, just show type names in a list; for instance, "Some.Nested.Namespace.SpecialClass".
		/// </summary>
		None,

		/// <summary>
		/// Group types by namespace and show foldout menus for nested namespaces; for
		/// instance, "Some > Nested > Namespace > SpecialClass".
		/// </summary>
		ByNamespace,

		/// <summary>
		/// Group types in the same way as Unity does for its component menu. This
		/// grouping method must only be used for <see cref="MonoBehaviour"/> types.
		/// </summary>
		ByAddComponentMenu
	}
}
