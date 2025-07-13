namespace com.ez.engine.core
{
    public interface ILateUpdatable : IService
    {
        void OnLateUpdate();
    }
}