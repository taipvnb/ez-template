using com.ez.engine.save.core;

namespace com.ez.engine.manager.save_load
{
	public class GameSaver : BaseGameSaver<GameSave, GameRuntime>
	{
		public GameSaver(GameSave gameSave, GameRuntime gameRuntime, IStorageProvider storageProvider) :
			base(gameSave, gameRuntime, storageProvider) { }
	}
}
