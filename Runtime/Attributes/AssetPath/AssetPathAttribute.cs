using System;
using UnityEngine;

namespace com.ez.engine.manager.ui
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class AssetPathAttribute : PropertyAttribute { }
}
