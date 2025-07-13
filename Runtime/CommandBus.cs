using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace com.ez.engine.command_bus
{
	public class CommandBus : ICommandBus
	{
		private readonly Dictionary<Type, ICommandHandler> _handlers = new();
		private readonly Dictionary<Type, Type> _handlerTypeLookup = new();

		public void Register<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand
		{
			var commandType = typeof(TCommand);
			var handlerType = handler.GetType();

			if (!_handlers.ContainsKey(commandType))
			{
				_handlers.Add(commandType, handler);
			}
			else
			{
				_handlers[commandType] = handler;
			}

			if (!_handlerTypeLookup.ContainsKey(handlerType))
			{
				_handlerTypeLookup.Add(handlerType, commandType);
			}
			else
			{
				_handlerTypeLookup[handlerType] = commandType;
			}
		}

		public void Register<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> handler) where TCommand : ICommand<TResponse>
		{
			var commandType = typeof(TCommand);
			var handlerType = handler.GetType();

			if (!_handlers.ContainsKey(commandType))
			{
				_handlers.Add(commandType, handler);
			}
			else
			{
				_handlers[commandType] = handler;
			}

			if (!_handlerTypeLookup.ContainsKey(handlerType))
			{
				_handlerTypeLookup.Add(handlerType, commandType);
			}
			else
			{
				_handlerTypeLookup[handlerType] = commandType;
			}
		}

		public void UnRegister<THandler>() where THandler : ICommandHandler
		{
			var handlerType = typeof(THandler);
			if (!_handlerTypeLookup.ContainsKey(handlerType))
			{
				throw new ArgumentException("Cannot remove command handler. Handler type was not found " + handlerType.FullName);
			}

			var commandType = _handlerTypeLookup[handlerType];
			_handlerTypeLookup.Remove(handlerType);
			_handlers.Remove(commandType);
		}

		public async UniTask Execute<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
		{
			var type = typeof(TCommand);
			if (_handlers.ContainsKey(type))
			{
				if (_handlers[type] is ICommandHandler<TCommand> handler)
				{
					await handler.Execute(command, cancellationToken);
				}
				else
				{
					throw new InvalidCastException($"Cannot cast handler from {_handlers[type].GetType()} to {typeof(ICommandHandler<TCommand>)}");
				}
			}
		}

		public async UniTask<TResponse> Execute<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
			where TCommand : ICommand<TResponse>
		{
			var type = typeof(TCommand);
			if (_handlers.ContainsKey(type))
			{
				if (_handlers[type] is ICommandHandler<TCommand, TResponse> handler)
				{
					return await handler.Execute(command, cancellationToken);
				}
				else
				{
					throw new InvalidCastException(
						$"[{GetType().Name}] Cannot cast handler from {_handlers[type].GetType()} to {typeof(ICommandHandler<TCommand>)}");
				}
			}

			return default(TResponse);
		}

		public void Clear()
		{
			_handlers.Clear();
		}
	}
}
