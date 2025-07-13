using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace com.ez.engine.services.scene
{
	public class LoadSceneOperationAddressable : LoadSceneOperationBase
	{
		private readonly Func<AsyncOperationHandle<SceneInstance>> _asyncOperationFunc;
		private AsyncOperationHandle<SceneInstance> _asyncOperationHandle;
		private bool _allowSceneActivation = true;
		private bool _hasExecuted = false;

		public LoadSceneOperationAddressable(Func<AsyncOperationHandle<SceneInstance>> asyncOperationFunc)
		{
			_asyncOperationFunc = asyncOperationFunc;
		}

		public override LoadSceneOperationHandle Execute()
		{
			if (_asyncOperationFunc == null)
			{
				throw new InvalidOperationException("Operation has not been initialized");
			}

			if (_hasExecuted)
			{
				throw new InvalidOperationException("Operation has already been executed");
			}

			_hasExecuted = true;
			_asyncOperationHandle = _asyncOperationFunc.Invoke();
			_asyncOperationHandle.Completed += HandleOnCompleted;
			return new LoadSceneOperationHandle(this);
		}

		public override void AllowSceneActivation(bool allowSceneActivation)
		{
			_allowSceneActivation = allowSceneActivation;
		}

		private void HandleOnCompleted(AsyncOperationHandle<SceneInstance> handle)
		{
			if (handle.Status == AsyncOperationStatus.Succeeded)
			{
				if (_allowSceneActivation)
				{
					var sceneInstance = handle.Result;
					sceneInstance.ActivateAsync();
				}

				OnCompleted?.Invoke();
			}
		}

		public override float Progress => _asyncOperationHandle.PercentComplete;
		public override bool IsDone => _asyncOperationHandle.IsDone;
		public override bool HasExecuted => _hasExecuted;

		public override event Action OnCompleted;
	}
}
