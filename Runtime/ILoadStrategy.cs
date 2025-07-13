using System.Collections.Generic;

namespace com.ez.engine.save.core
{
	public interface ILoadStrategy
	{
		IEnumerable<int> ConverterIds { get; }

		IEnumerable<IVersionConverter> Converters { get; }

		void Load(ISaveModel saveModel, IRuntimeModel runtimeModel, bool firstLoad);
	}

	public interface ILoadStrategy<in TSaveModel, in TRuntimeModel> : ILoadStrategy
		where TSaveModel : ISaveModel
		where TRuntimeModel : IRuntimeModel
	{
		void Load(TSaveModel saveModel, TRuntimeModel runtimeModel, bool firstLoad);
	}
}
