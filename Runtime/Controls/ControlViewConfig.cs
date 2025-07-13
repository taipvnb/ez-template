namespace com.ez.engine.manager.ui
{
	public readonly struct ControlViewConfig
	{
		public readonly ViewConfig Config;

		public ControlViewConfig(in ViewConfig options)
		{
			Config = options;
		}

		public ControlViewConfig(string resourcePath
			, bool playAnimation = true
			, bool loadAsync = true
			, PoolingPolicy poolingPolicy = PoolingPolicy.UseSettings)
		{
			Config = new ViewConfig(resourcePath, playAnimation, loadAsync, poolingPolicy);
		}

		public static implicit operator ControlViewConfig(in ViewConfig config) => new(config);

		public static implicit operator ControlViewConfig(string assetPath) => new(new ViewConfig(assetPath));

		public static implicit operator ViewConfig(in ControlViewConfig config) => config.Config;
	}
}
