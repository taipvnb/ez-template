using com.ez.engine.save.core;

namespace com.ez.engine.save.file
{
	public interface IFileStorage : IStorageProvider
	{
		string GetFilePath(string fileName);
	}
}
