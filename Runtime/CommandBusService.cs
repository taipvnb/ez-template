using System.Threading;
using com.ez.engine.command_bus;
using com.ez.engine.core;
using com.ez.engine.core.di;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ez.engine.services.command_bus
{
	[Service(typeof(ICommandBusService))]
	public class CommandBusService : MonoBehaviour, ICommandBusService
	{
		public int Priority => 0;
		public bool Initialized { get; set; }

		private IInjector _injector;
		private ICommandBus _commandBus;

		public UniTask OnInitialize(IArchitecture architecture)
		{
			_injector = architecture.Injector;
			_commandBus = new CommandBus();
			Initialized = true;
			return UniTask.CompletedTask;
		}

		public void Register<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand
		{
			_injector?.Resolve(handler);
			_commandBus.Register(handler);
		}

		public void Register<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> handler) where TCommand : ICommand<TResponse>
		{
			_injector?.Resolve(handler);
			_commandBus.Register(handler);
		}

		public void UnRegister<THandler>() where THandler : ICommandHandler
		{
			_commandBus.UnRegister<THandler>();
		}

		public UniTask Execute<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
		{
			return _commandBus.Execute(command, cancellationToken);
		}

		public UniTask<TResponse> Execute<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
			where TCommand : ICommand<TResponse>
		{
			return _commandBus.Execute<TCommand, TResponse>(command, cancellationToken);
		}

		public void Clear()
		{
			_commandBus.Clear();
		}
	}
}
