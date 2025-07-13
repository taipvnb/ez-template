using System;

namespace com.ez.engine.save.core
{
	public interface IGameSave
	{
		ISaveModel GetModel(Type saveModelType);

		TSaveModel GetModel<TSaveModel>() where TSaveModel : ISaveModel;

		bool TryGetModel(Type saveModelType, out ISaveModel saveModel);

		bool TryGetModel<TSaveModel>(out TSaveModel saveModel) where TSaveModel : ISaveModel;

		bool TrySetModel<TSaveModel>(TSaveModel saveModel) where TSaveModel : ISaveModel;
	}
}
