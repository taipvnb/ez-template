using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace com.ez.engine.services.command_bus.editor
{
    public class BuildPreProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1;

        private const string SettingName = "com.unimob.services.command_bus";
        private const string PackageName = "com.unimob.services.command-bus";

        public void OnPreprocessBuild(BuildReport report)
        {
            var pluginPath = Path.Combine(Application.dataPath, $"Plugins/Unimob/Settings/{SettingName}");
            if (!Directory.Exists(pluginPath))
            {
                Directory.CreateDirectory(pluginPath);
            }

            if (AssetDatabase.IsValidFolder($"Packages/{PackageName}"))
            {
                AssetDatabase.CopyAsset($"Packages/{PackageName}/Runtime/link.xml", $"Assets/Plugins/Unimob/Settings/{SettingName}/link.xml");
            }
        }
    }
}
