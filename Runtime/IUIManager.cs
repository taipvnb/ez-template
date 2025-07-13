using System;
using System.Collections.Generic;
using com.ez.engine.core.di;
using com.ez.engine.core.manager;

namespace com.ez.engine.manager.ui
{
	public interface IUIManager : IManager
	{
		IInjector Injector { get; }

		List<IWindowContainer> Containers { get; }

		List<IViewPresenter> Presenters { get; }

		Action<IView> ViewWillOpen { get; set; }
		
		Action<IView> ViewOpened { get; set; }
		
		Action<IView> ViewWillClose { get; set; }
		
		Action<IView> ViewClosed { get; set; }

		void AddContainer(IWindowContainer container);

		bool RemoveContainer(IWindowContainer container);

		TContainer GetContainer<TContainer>() where TContainer : IWindowContainer;

		TContainer GetContainer<TContainer>(string containerName) where TContainer : IWindowContainer;

		bool TryGetContainer<TContainer>(out TContainer container) where TContainer : IWindowContainer;

		bool TryGetContainer<TContainer>(string containerName, out TContainer container) where TContainer : IWindowContainer;

		bool HasPresenter<TPresenter>() where TPresenter : IViewPresenter;

		void AddPresenter(IViewPresenter viewPresenter);

		void RemovePresenter(IViewPresenter viewPresenter);

		TPresenter GetPresenter<TPresenter>() where TPresenter : IViewPresenter;

		public IViewPresenter GetPresenter(Type presenterType);

		bool TryGetPresenter<TPresenter>(out TPresenter viewPresenter) where TPresenter : IViewPresenter;

		public bool TryGetPresenter(Type presenterType, out IViewPresenter viewPresenter);
	}
}
