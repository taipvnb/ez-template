using System.Threading;
using Cysharp.Threading.Tasks;
using com.ez.engine.core;
using com.ez.engine.command_bus;

namespace com.ez.engine.services.command_bus
{
    public interface ICommandBusService : IService, IInitializable
    {
        void Register<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand;

        void Register<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> handler) where TCommand : ICommand<TResponse>;

        void UnRegister<THandler>() where THandler : ICommandHandler;

        UniTask Execute<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;

        UniTask<TResponse> Execute<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand<TResponse>;

        void Clear();
    } 
}