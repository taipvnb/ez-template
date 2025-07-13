using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ez.engine.foundation.animation
{
	public static class AnimationExtensions
	{
		public static async UniTask PlayAsync(this IAnimation self, IProgress<float> progress = null)
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

		public static AnimationPlayer Play(this IAnimation self, IProgress<float> progress = null, CancellationToken cancellationToken = default)
		{
			var player = new AnimationPlayer(self);
			progress?.Report(0.0f);
			player.Play();

			_ = RunAsync(player, progress, cancellationToken).AttachExternalCancellation(cancellationToken);

			return player;
		}

		private static async UniTask RunAsync(AnimationPlayer player, IProgress<float> progress, CancellationToken cancellationToken)
		{
			while (player.IsFinished == false)
			{
				cancellationToken.ThrowIfCancellationRequested();
				await UniTask.NextFrame(cancellationToken);
				player.Update(Time.unscaledDeltaTime);
				progress?.Report(player.Time / player.Animation.Duration);
			}
		}
	}
}
