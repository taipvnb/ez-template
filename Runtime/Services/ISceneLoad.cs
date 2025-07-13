namespace com.ez.engine.core
{
    public interface ISceneLoad : IService
    {
        void OnSceneLoad(string sceneName);

        void OnSceneUnload(string sceneName);
    }
}