using System;
using Cysharp.Threading.Tasks;

namespace com.ez.engine.save.core
{
	public interface IGameLoader
	{
		bool Load(Type saveModelType);

		bool Load<TSaveModel>() where TSaveModel : ISaveModel, new();

		UniTask<bool> LoadAsync(Type saveModelType);

		UniTask<bool> LoadAsync<TDataModel>() where TDataModel : ISaveModel, new();

		bool LoadFromRawData(string rawData);
	}
}
