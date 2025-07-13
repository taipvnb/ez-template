using UnityEditor;

namespace com.ez.engine.manager.ui
{
	[CustomEditor(typeof(UnityEngine.UIElements.ProgressBar))]
	[CanEditMultipleObjects]
	public sealed class ProgressBarEditor : ProgressBarEditorBase
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawValueFields();

			EditorGUILayout.Space();

			var fillModeProperty = serializedObject.FindProperty("_fillMode");
			EditorGUILayout.PropertyField(fillModeProperty);
			if (fillModeProperty.enumValueIndex == 0)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_fillImage"));
			}
			else
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_fillRect"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("_direction"));
			}

			EditorGUILayout.Space();

			DrawOnValueChangedEvent();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
