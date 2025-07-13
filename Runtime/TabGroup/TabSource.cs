using System;
using com.ez.engine.utils.class_type_reference;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace com.ez.engine.manager.ui
{
	[Serializable]
	public class TabSource
	{
		[SerializeField, FoldoutGroup("$_sheetView")] private Button _button;
		[SerializeField, FoldoutGroup("$_sheetView")] private SheetView _sheetView;

		[SerializeField, ClassImplements(typeof(ISheetPresenter)), FoldoutGroup("$_sheetView")]
		private ClassTypeReference _sheetPresenterType;

		public Button Button => _button;

		public SheetView SheetView => _sheetView;

		public Type SheetPresenterType => _sheetPresenterType.Type;
	}
}
