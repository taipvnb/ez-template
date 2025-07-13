using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.ez.engine.manager.ui
{
	[Serializable]
	public class TabGroup : MonoBehaviour
	{
		[SerializeField] private SheetContainer _sheetContainer;
		[SerializeField] private List<TabItem> _items;
		[SerializeField] private List<TabSource> _sources;
		[SerializeField] private int _initialIndex;
		[SerializeField] private bool _playAnimation;

		public event Action<int> OnTabChanged;

		private readonly List<IViewPresenter> _children = new();

		[ShowInInspector, ReadOnly] private readonly Dictionary<int, int> _indexToSheetIdMap = new();

		private bool _isInitializing;
		private bool _isInitialized;

		public async UniTask InitializeAsync(IUIManager uiManager)
		{
			if (_isInitialized)
			{
				throw new InvalidOperationException($"{nameof(TabGroup)} is already initialized.");
			}

			if (_isInitializing)
			{
				throw new InvalidOperationException($"{nameof(TabGroup)} is initializing.");
			}

			_isInitializing = true;

			for (var i = 0; i < _sources.Count; i++)
			{
				var index = i;
				var source = _sources[i];
				var sheetPresenter = Activator.CreateInstance(source.SheetPresenterType, uiManager, source.SheetView);
				uiManager.Injector.Resolve(sheetPresenter);
				_children.Add(sheetPresenter as IViewPresenter);

				source.Button.onClick.RemoveAllListeners();
				source.Button.onClick.AddListener(() =>
				{
					if (_sheetContainer.IsInTransition)
					{
						return;
					}

					if (_sheetContainer.ActiveSheetId == _indexToSheetIdMap[index])
					{
						return;
					}

					SetActiveTabAsync(index, _playAnimation).Forget();
				});

				var sheetId = await _sheetContainer.RegisterAsync(source.SheetView);
				_indexToSheetIdMap.Add(index, sheetId);
			}

			_isInitialized = true;
			_isInitializing = false;

			await SetActiveTabAsync(_initialIndex, _playAnimation);
		}

		public int GetSheetIdFromIndex(int index)
		{
			return _indexToSheetIdMap[index];
		}

		public async UniTask SetActiveTabAsync(int index, bool playAnimation)
		{
			SetActiveTab(index);
			OnTabChanged?.Invoke(index);
			var sheetId = GetSheetIdFromIndex(index);
			await _sheetContainer.ShowAsync(sheetId, playAnimation);
		}

		public void Dispose()
		{
			foreach (var child in _children)
			{
				child.Dispose();
			}
		}

		private void SetActiveTab(int index)
		{
			foreach (var tab in _items)
			{
				tab.SetActive(index);
			}
		}
	}
}
