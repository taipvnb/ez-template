using UnityEditor;

namespace com.ez.engine.services.scene.editor
{
    internal class PackageInstaller
    {
        [MenuItem("EzEngine/Services/Settings/Scene Management")]
        private static void SelectionSettings()
        {
            Selection.activeObject = SceneServiceSettings.Instance;
        }
    }
}