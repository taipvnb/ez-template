using System.Collections.Generic;
using UnityEngine;

namespace com.ez.engine.save.core
{
	public abstract class BaseLoadStrategy<TSaveModel, TRuntimeModel> : ILoadStrategy<TSaveModel, TRuntimeModel>
		where TSaveModel : ISaveModel
		where TRuntimeModel : IRuntimeModel
	{
		public IEnumerable<int> ConverterIds => _converters.Keys;
		public IEnumerable<IVersionConverter> Converters => _converters.Values;

		private readonly Dictionary<int, IVersionConverter> _converters = new();

		protected void AddConverter(IVersionConverter converter)
		{
			_converters.Add(converter.FromVersion, converter);
		}

		public void Load(ISaveModel saveModel, IRuntimeModel runtimeModel, bool firstLoad)
		{
			if (saveModel is TSaveModel typedSaveModel && runtimeModel is TRuntimeModel typedDataModel)
			{
				Load(typedSaveModel, typedDataModel, firstLoad);
			}
			else
			{
				Debug.LogError("Type mismatch: Cannot cast ISaveModel or IRuntimeModel to the expected types.");
			}
		}

		public void Load(TSaveModel saveModel, TRuntimeModel runtimeModel, bool firstLoad)
		{
			switch (firstLoad)
			{
				case true:
					OnFirstLoad(runtimeModel);
					break;
				case false:
					var rawSaveModel = saveModel;
					var currentVersion = rawSaveModel.Version;
					var targetVersion = runtimeModel.Version;
					if (currentVersion < targetVersion)
					{
						while (currentVersion < targetVersion)
						{
							var converter = (IVersionConverter<TSaveModel, TSaveModel>)_converters[currentVersion];
							rawSaveModel = converter.Convert(saveModel);
							currentVersion = converter.ToVersion;
						}
					}

					OnLoad(rawSaveModel, runtimeModel);
					break;
			}
		}

		protected abstract void OnFirstLoad(TRuntimeModel runtimeModel);

		protected abstract void OnLoad(TSaveModel saveModel, TRuntimeModel runtimeModel);
	}
}
