namespace com.ez.engine.command_bus
{
    public interface ICommand { }

    public interface ICommand<out TResponse> : ICommand { }
}