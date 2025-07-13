using System;
using Cysharp.Threading.Tasks;

namespace com.ez.engine.assets.loader.core
{
	public interface IAssetRequest<TAsset>
	{
		void SetStatus(AssetRequestStatus status);

		void SetResult(TAsset result);

		void SetProgressFunc(Func<float> progress);

		void SetTask(UniTask<TAsset> task);

		void SetOperationException(Exception ex);
	}
}
