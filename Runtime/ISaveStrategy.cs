namespace com.ez.engine.save.core
{
	public interface ISaveStrategy
	{
		ISaveModel Save(IRuntimeModel runtimeModel);
	}

	public interface ISaveStrategy<out TSaveModel, in TRuntimeModel> : ISaveStrategy
		where TRuntimeModel : IRuntimeModel
		where TSaveModel : ISaveModel
	{
		TSaveModel Save(TRuntimeModel runtimeModel);
	}
}
