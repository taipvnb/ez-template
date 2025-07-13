using UnityEditor;

namespace com.ez.engine.services.device.editor
{
    internal class PackageInstaller
    {
        private const string PackageName = "com.ez.engine.services.device";

        [MenuItem("EzEngine/Services/Settings/Device")]
        private static void SelectionSettings()
        {
            Selection.activeObject = DeviceServiceSettings.Instance;
        }
    }
}