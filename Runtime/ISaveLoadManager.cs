using com.ez.engine.core.manager;
using com.ez.engine.save.core;
using Cysharp.Threading.Tasks;

namespace com.ez.engine.manager.save_load
{
	public interface ISaveLoadManager : IManager
	{
		GameSaver GameSaver { get; }

		GameLoader GameLoader { get; }

		TRuntimeModel GetRuntimeModel<TRuntimeModel>() where TRuntimeModel : IRuntimeModel;

		bool TryGetRuntimeModel<TRuntimeModel>(out TRuntimeModel runtimeModel) where TRuntimeModel : IRuntimeModel;

		TSaveModel GetSaveModel<TSaveModel>() where TSaveModel : class, ISaveModel;

		bool TryGetSaveModel<TSaveModel>(out TSaveModel saveModel) where TSaveModel : class, ISaveModel;

		bool Save<TSaveModel>() where TSaveModel : class, ISaveModel;

		UniTask<bool> SaveAsync<TSaveModel>() where TSaveModel : class, ISaveModel;

		bool Load<TDataModel>() where TDataModel : ISaveModel, new();

		UniTask<bool> LoadAsync<TDataModel>() where TDataModel : ISaveModel, new();

		void SaveAll();
	}
}
