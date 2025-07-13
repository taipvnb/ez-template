using System;
using com.ez.engine.foundation.animation;

namespace com.ez.engine.services.scene
{
	public interface ISceneTransition : IAnimation
	{
		TransitionType Type { get; }

		void Initialize();
	}
}
