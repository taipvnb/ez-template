namespace com.ez.engine.save.core
{
	public interface IVersionConverter
	{
		int FromVersion { get; }

		int ToVersion { get; }
	}

	public interface IVersionConverter<in TFrom, out TTo> : IVersionConverter where TFrom : ISaveModel where TTo : ISaveModel
	{
		TTo Convert(TFrom from);
	}
}
