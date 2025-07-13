using System;
using UnityEditor;

namespace com.ez.engine.manager.ui
{
	public struct MixedValueScope : IDisposable
	{
		private bool _isDisposed;
		private readonly bool prevMixedValue;

		public MixedValueScope(bool showMixedValue)
		{
			_isDisposed = false;
			prevMixedValue = EditorGUI.showMixedValue;
			EditorGUI.showMixedValue = showMixedValue;
		}

		public void Dispose()
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				EditorGUI.showMixedValue = prevMixedValue;
			}
		}
	}
}
