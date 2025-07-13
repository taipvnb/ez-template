using System;
using UnityEngine;

namespace com.ez.engine.manager.ui
{
	[Serializable]
	public class PanelTransitionAnimationContainer
	{
		[SerializeField] private TransitionAnimation _enterAnimation = new();
		[SerializeField] private TransitionAnimation _exitAnimation = new();

		public TransitionAnimation EnterAnimation => _enterAnimation;
		public TransitionAnimation ExitAnimation => _exitAnimation;

		public TransitionAnimation GetTransition(bool enter)
		{
			return enter ? _enterAnimation : _exitAnimation;
		}

		public ITransitionAnimation GetAnimation(bool enter)
		{
			var transition = enter ? _enterAnimation : _exitAnimation;
			return transition.GetAnimation();
		}
	}
}
