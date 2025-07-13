using UnityEngine;

namespace com.ez.engine.assets.loader.core
{
	public abstract class AssetLoaderObject : ScriptableObject, IAssetLoader
	{
		public abstract AssetRequest<TAsset> Load<TAsset>(string address) where TAsset : Object;
		public abstract AssetRequest<Object> Load(string address);

		public abstract AssetRequest<TAsset> LoadAsync<TAsset>(string address) where TAsset : Object;
		public abstract AssetRequest<Object> LoadAsync(string address);

		public abstract void Release(AssetRequest request);
	}
}
