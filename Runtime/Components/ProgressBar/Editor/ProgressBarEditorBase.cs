using System.Linq;
using UnityEditor;

namespace com.ez.engine.manager.ui
{
	public abstract class ProgressBarEditorBase : Editor
	{
		private const string UndoMessageMinValue = "Change ProgressBar Min Value";
		private const string UndoMessageMaxValue = "Change ProgressBar Max Value";
		private const string UndoMessageValue = "Change ProgressBar Value";

		private void OnEnable()
		{
			Notify();
			Undo.undoRedoPerformed += Notify;
		}

		private void OnDisable()
		{
			Undo.undoRedoPerformed -= Notify;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawValueFields();
			EditorGUILayout.Space();
			DrawOnValueChangedEvent();

			serializedObject.ApplyModifiedProperties();
		}

		protected void DrawOnValueChangedEvent()
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("onValueChanged"));
		}

		protected void DrawValueFields()
		{
			var minValueProperty = serializedObject.FindProperty("minValue");
			using (var changeCheck = new EditorGUI.ChangeCheckScope())
			{
				using (new MixedValueScope(minValueProperty.hasMultipleDifferentValues))
				{
					var fieldValue = EditorGUILayout.DelayedFloatField("Min Value", minValueProperty.floatValue);
					if (changeCheck.changed)
					{
						Undo.RecordObjects(targets, UndoMessageMinValue);
						foreach (var target in targets.Select(x => (ProgressBarBase)x))
						{
							if (fieldValue <= target.MaxValue)
							{
								target.MinValue = fieldValue;
							}
						}
					}
				}
			}

			var maxValueProperty = serializedObject.FindProperty("maxValue");
			using (var changeCheck = new EditorGUI.ChangeCheckScope())
			{
				using (new MixedValueScope(maxValueProperty.hasMultipleDifferentValues))
				{
					var fieldValue = EditorGUILayout.DelayedFloatField("Max Value", maxValueProperty.floatValue);
					if (changeCheck.changed)
					{
						Undo.RecordObjects(targets, UndoMessageMaxValue);
						foreach (var target in targets.Select(x => (ProgressBarBase)x))
						{
							if (fieldValue >= target.MinValue)
							{
								target.MaxValue = fieldValue;
							}
						}
					}
				}
			}

			var valueProperty = serializedObject.FindProperty("value");
			using (var changeCheck = new EditorGUI.ChangeCheckScope())
			{
				using (new MixedValueScope(valueProperty.hasMultipleDifferentValues))
				{
					var fieldValue = EditorGUILayout.Slider("Value", valueProperty.floatValue, minValueProperty.floatValue,
						maxValueProperty.floatValue);
					if (changeCheck.changed)
					{
						Undo.RecordObjects(targets, UndoMessageValue);
						foreach (var target in targets.Select(x => (ProgressBarBase)x))
						{
							target.Value = fieldValue;
						}
					}
				}
			}
		}

		private void Notify()
		{
			foreach (var target in targets.Select(x => (ProgressBarBase)x))
			{
				target.ForceNotify();
			}
		}
	}
}
