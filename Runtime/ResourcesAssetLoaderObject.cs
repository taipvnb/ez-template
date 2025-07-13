using com.ez.engine.assets.loader.core;
using UnityEngine;

namespace com.ez.engine.assets.loader.resources
{
	[CreateAssetMenu(fileName = "ResourcesAssetLoader", menuName = "EzEngine/Asset Loader/Resources Asset Loader")]
	public class ResourcesAssetLoaderObject : AssetLoaderObject, IAssetLoader
	{
		private readonly ResourcesAssetLoader _loader = new ResourcesAssetLoader();

		public override AssetRequest<TAsset> Load<TAsset>(string address)
		{
			return _loader.Load<TAsset>(address);
		}

		public override AssetRequest<Object> Load(string address)
		{
			return _loader.Load(address);
		}

		public override AssetRequest<TAsset> LoadAsync<TAsset>(string address)
		{
			return _loader.Load<TAsset>(address);
		}

		public override AssetRequest<Object> LoadAsync(string address)
		{
			return _loader.Load(address);
		}

		public override void Release(AssetRequest request)
		{
			_loader.Release(request);
		}
	}
}
