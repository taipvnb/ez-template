using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace com.ez.engine.plugins.bg_database.editor
{
	public class BuildPreprocessor : IPreprocessBuildWithReport
	{
		public int callbackOrder => 0;

		private const string PackagesFolderPath = "Packages/com.unimob.plugins.bg-database";

		public void OnPreprocessBuild(BuildReport report)
		{
			if (!AssetDatabase.IsValidFolder("Assets/BGDatabase"))
			{
				AssetDatabase.CreateFolder("Assets", "BGDatabase");
			}

			if (AssetDatabase.IsValidFolder($"{PackagesFolderPath}/BGDatabase"))
			{
				AssetDatabase.CopyAsset("Packages/com.unimob.plugins.facebook-sdk/BGDatabase/link.xml", "Assets/BGDatabase/link.xml");
			}
		}
	}
}
