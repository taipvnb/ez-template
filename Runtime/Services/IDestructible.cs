namespace com.ez.engine.core
{
    public interface IDestructible : IService
    {
        bool WillDestroy { get; set; }
        void OnWillDestroy();
    }
}