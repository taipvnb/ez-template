using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace com.ez.engine.services.device.editor
{
	public class BuildPreprocessor : IPreprocessBuildWithReport
	{
		public int callbackOrder => 1;

		private const string PackageName = "com.ez.engine.services.device";

		public void OnPreprocessBuild(BuildReport report)
		{
			var pluginPath = Path.Combine(Application.dataPath, $"Plugins/EzEngine/Settings/{PackageName}");
			if (!Directory.Exists(pluginPath))
			{
				Directory.CreateDirectory(pluginPath);
			}

			if (AssetDatabase.IsValidFolder($"Packages/{PackageName}"))
			{
				AssetDatabase.CopyAsset($"Packages/{PackageName}/Runtime/link.xml", $"Assets/Plugins/EzEngine/Settings/{PackageName}/link.xml");
			}
		}
	}
}
