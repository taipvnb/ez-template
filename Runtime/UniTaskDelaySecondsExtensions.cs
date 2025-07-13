using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.unimob.unitask.extensions
{
    public static class UniTaskDelaySecondsExtensions
    {
        public static UniTask DelaySeconds(this GameObject self, double seconds, bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
        {
            return UniTask.DelaySeconds
            (
                gameObject: self,
                seconds: seconds,
                ignoreTimeScale: ignoreTimeScale,
                delayTiming: delayTiming,
                cancellationToken: cancellationToken
            );
        }

        public static UniTask DelaySeconds(this Component self, double seconds, bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
        {
            return UniTask.DelaySeconds
            (
                component: self,
                seconds: seconds,
                ignoreTimeScale: ignoreTimeScale,
                delayTiming: delayTiming,
                cancellationToken: cancellationToken
            );
        }

        public static UniTask DelaySecondsIf(this Component self, bool condition, double seconds, bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update, CancellationToken cancellationToken = default)
        {
            if (!condition) return UniTask.CompletedTask;

            return self.DelaySeconds
            (
                seconds: seconds,
                ignoreTimeScale: ignoreTimeScale,
                delayTiming: delayTiming,
                cancellationToken: cancellationToken
            );
        }

        public static UniTask DelaySeconds(this GameObject self, double seconds, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            return UniTask.DelaySeconds
            (
                gameObject: self,
                seconds: seconds,
                delayType: delayType,
                delayTiming: delayTiming,
                cancellationToken: cancellationToken
            );
        }

        public static UniTask DelaySeconds(this Component self, double seconds, DelayType delayType, PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            return UniTask.DelaySeconds
            (
                component: self,
                seconds: seconds,
                delayType: delayType,
                delayTiming: delayTiming,
                cancellationToken: cancellationToken
            );
        }
    }
}