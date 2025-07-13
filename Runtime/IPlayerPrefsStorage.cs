using com.ez.engine.save.core;

namespace com.ez.engine.save.playerprefs
{
	public interface IPlayerPrefsStorage : IStorageProvider
	{
		void SetFloat(string key, float value);

		float GetFloat(string key);

		void SetInt(string key, int value);

		int GetInt(string key);

		void SetString(string key, string value);

		string GetString(string key);
	}
}
