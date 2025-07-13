namespace com.ez.engine.manager.ui
{
	public static class ViewConfigExtensions
	{
		public static TViewConfig As<TViewConfig>(this IViewConfig viewConfig) where TViewConfig : IViewConfig
		{
			return (TViewConfig)viewConfig;
		}
	}
}
