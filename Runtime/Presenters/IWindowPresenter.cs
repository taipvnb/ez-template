using System;

namespace com.ez.engine.manager.ui
{
	public interface IWindowPresenter : IViewPresenter
	{
		IViewConfig ViewConfig { get; }

		IViewContainer ViewContainer { get; }

		IViewPresenter GetChild(Type type);

		bool TryGetChild<TPresenter>(out TPresenter childPresenter) where TPresenter : IViewPresenter;
	}
}
