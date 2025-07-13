using UnityEngine;

namespace com.ez.engine.core.di
{
	internal static class ResourcesUtilities
	{
		internal static bool TryLoad<T>(string path, out T resource) where T : Object
		{
			resource = Resources.Load<T>(path);
			return resource != null;
		}
	}
}
