using UnityEngine;

namespace com.ez.engine.save.core
{
	public abstract class BaseSaveStrategy<TSaveModel, TRuntimeModel> : ISaveStrategy<TSaveModel, TRuntimeModel>
		where TRuntimeModel : IRuntimeModel
		where TSaveModel : ISaveModel
	{
		public ISaveModel Save(IRuntimeModel runtimeModel)
		{
			if (runtimeModel is TRuntimeModel typedRuntimeModel)
			{
				return Save(typedRuntimeModel);
			}
			else
			{
				Debug.LogError("Type mismatch: Cannot cast IRuntimeModel to the expected type.");
				return null;
			}
		}

		public abstract TSaveModel Save(TRuntimeModel runtimeModel);
	}
}
