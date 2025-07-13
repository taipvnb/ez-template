using com.ez.engine.save.core;

namespace com.ez.engine.manager.save_load
{
	public class GameLoader : BaseGameLoader<GameSave, GameRuntime>
	{
		public GameLoader(GameSave gameSave, GameRuntime gameRuntime, IStorageProvider storageProvider) :
			base(gameSave, gameRuntime, storageProvider) { }
	}
}
