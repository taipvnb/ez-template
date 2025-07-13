using System;

namespace com.ez.engine.save.core
{
	public interface IGameRuntime
	{
		IRuntimeModel GetModel(Type runtimeModelType);

		TRuntimeModel GetModel<TRuntimeModel>() where TRuntimeModel : IRuntimeModel;

		bool TryGetModel(Type runtimeModelType, out IRuntimeModel runtimeModel);

		bool TryGetModel<TRuntimeModel>(out TRuntimeModel runtimeModel) where TRuntimeModel : IRuntimeModel;
	}
}
