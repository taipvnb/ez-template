namespace com.ez.engine.core
{
    public interface ILowMemory : IService
    {
        void OnLowMemory();
    }
}