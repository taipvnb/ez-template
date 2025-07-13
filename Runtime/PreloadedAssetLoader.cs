using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace com.ez.engine.assets.loader.core
{
	public class PreloadedAssetLoader : IAssetLoader
	{
		public Dictionary<string, Object> PreloadedAssets { get; } = new Dictionary<string, Object>();

		private int _nextRequestId;

		public AssetRequest<TAsset> Load<TAsset>(string address) where TAsset : Object
		{
			var requestId = _nextRequestId++;

			var request = new AssetRequest<TAsset>(requestId);
			var setter = (IAssetRequest<TAsset>)request;

			TAsset result = null;
			if (PreloadedAssets.TryGetValue(address, out var obj))
			{
				result = obj as TAsset;
			}

			setter.SetResult(result);

			var status = result != null ? AssetRequestStatus.Succeeded : AssetRequestStatus.Failed;
			setter.SetStatus(status);

			if (result == null)
			{
				var exception = new InvalidOperationException($"Requested asset（Key: {address}）was not found.");
				setter.SetOperationException(exception);
			}

			setter.SetProgressFunc(() => 1.0f);
			setter.SetTask(UniTask.FromResult(result));
			return request;
		}

		public AssetRequest<Object> Load(string address)
		{
			return Load<Object>(address);
		}

		public AssetRequest<TAsset> LoadAsync<TAsset>(string address) where TAsset : Object
		{
			return Load<TAsset>(address);
		}

		public AssetRequest<Object> LoadAsync(string address)
		{
			return Load<Object>(address);
		}

		public void Release(AssetRequest request) { }

		public void AddAsset(Object asset)
		{
			if (!PreloadedAssets.ContainsKey(asset.name))
			{
				PreloadedAssets.Add(asset.name, asset);
			}
			else
			{
				Debug.LogWarning("The asset has already been added. (Key: " + asset.name + ")");
			}
		}
	}
}
