using UnityEngine;

namespace com.ez.engine.assets.loader.core
{
	public interface IAssetLoader
	{
		AssetRequest<TAsset> Load<TAsset>(string address) where TAsset : Object;

		AssetRequest<Object> Load(string address);
		
		AssetRequest<TAsset> LoadAsync<TAsset>(string address) where TAsset : Object;

		AssetRequest<Object> LoadAsync(string address);
		
		void Release(AssetRequest request);
	}
}
