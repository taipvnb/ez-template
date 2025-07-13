namespace com.ez.engine.save.core
{
	public interface IRuntimeModel
	{
		int Version { get; }
		
		int SaveThreshold { get; }
	}
}
