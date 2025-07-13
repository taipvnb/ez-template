namespace com.ez.engine.core
{
    public interface IFocusable : IService
    {
        void OnAppFocus(bool focus);
    }
}