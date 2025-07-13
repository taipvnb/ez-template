using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace com.ez.engine.services.scene
{
	public class AddressableSceneLoader : ISceneLoader
	{
		private AsyncOperationHandle<SceneInstance> _asyncOperationHandle;

		public void Load(string sceneName, LoadSceneMode loadSceneMode)
		{
			_asyncOperationHandle = Addressables.LoadSceneAsync(sceneName, loadSceneMode);
		}

		public LoadSceneOperationHandle LoadAsync(string sceneName, LoadSceneMode loadSceneMode)
		{
			var operation = GetLoadSceneOperation(sceneName, loadSceneMode);
			return operation.Execute();
		}

		public void Unload(string sceneName)
		{
			if (!_asyncOperationHandle.IsValid() || !_asyncOperationHandle.Result.Scene.isLoaded)
			{
				return;
			}

			Addressables.UnloadSceneAsync(_asyncOperationHandle);
		}

		public LoadSceneOperationHandle UnloadAsync(string sceneName)
		{
			var operation = GetUnloadSceneOperation(sceneName);
			return operation.Execute();
		}

		private LoadSceneOperationAddressable GetLoadSceneOperation(string sceneName, LoadSceneMode loadSceneMode)
		{
			return new LoadSceneOperationAddressable(() => _asyncOperationHandle = Addressables.LoadSceneAsync(sceneName, loadSceneMode));
		}

		private LoadSceneOperationAddressable GetUnloadSceneOperation(string sceneName)
		{
			if (!_asyncOperationHandle.IsValid() || !_asyncOperationHandle.Result.Scene.isLoaded)
			{
				return default;
			}

			return new LoadSceneOperationAddressable(() => Addressables.UnloadSceneAsync(_asyncOperationHandle));
		}
	}
}
