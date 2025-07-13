#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace com.ez.engine.foundation
{
    [CreateAssetMenu(menuName = "Unimob/Build info settings", fileName = "BuildInfoSettings")]
    public class BuildInfo : ScriptableObject
    {
        public static BuildInfo Instance => _instance == null ? LoadInfo() : _instance;
        private static BuildInfo _instance;

        public string Version;
        public string BuildId;

        private const string FilePath = "Assets/Resources/BuildInfoSettings.asset";

        private static BuildInfo LoadInfo()
        {
            var result = Resources.Load<BuildInfo>("BuildInfoSettings");
            if (result == null)
            {
#if UNITY_EDITOR
                var directory = Path.GetDirectoryName(FilePath);

                if (directory == null)
                {
                    throw new InvalidOperationException($"Failed to get directory from package settings path: {FilePath}");
                }

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                result = CreateInstance<BuildInfo>();
                AssetDatabase.CreateAsset(result, FilePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#else
                Debug.LogWarning("You need create asset file before build from context menu \"Assets/Create/Build info settings\"");
#endif
            }

            return result;
        }
    }
}