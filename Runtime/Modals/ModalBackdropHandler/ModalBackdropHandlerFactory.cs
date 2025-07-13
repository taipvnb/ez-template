using System;

namespace com.ez.engine.manager.ui
{
	internal static class ModalBackdropHandlerFactory
	{
		public static IModalBackdropHandler Create(ModalBackdropStrategy strategy, ModalBackdrop prefab)
		{
			return strategy switch
			{
				ModalBackdropStrategy.GeneratePerModal => new GeneratePerModalModalBackdropHandler(prefab),
				_ => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
			};
		}
	}
}
