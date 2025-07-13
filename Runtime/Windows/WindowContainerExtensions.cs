namespace com.ez.engine.manager.ui
{
	public static class WindowContainerExtensions
	{
		public static TContainer As<TContainer>(this IViewContainer container) where TContainer : IViewContainer
		{
			return (TContainer)container;
		}
	}
}
