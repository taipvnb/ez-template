namespace com.ez.engine.core
{
    public interface IPausable : IService
    {
        void OnAppPause(bool pause);
    }
}