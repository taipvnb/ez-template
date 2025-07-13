using System;
using Cysharp.Threading.Tasks;
using  com.ez.engine.foundation.animation;
using UnityEngine;

namespace com.ez.engine.manager.ui
{
	internal static class TransitionAnimationExtensions
	{
		public static async UniTask PlayAsync(this ITransitionAnimation self, IProgress<float> progress = null)
		{
			var player = new AnimationPlayer(self);
			progress?.Report(0.0f);
			player.Play();

			while (player.IsFinished == false)
			{
				await UniTask.NextFrame();
				player.Update(Time.unscaledDeltaTime);
				progress?.Report(player.Time / self.Duration);
			}
		}
	}
}
