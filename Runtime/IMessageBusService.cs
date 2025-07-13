using System;
using com.ez.engine.core;
using com.ez.engine.message_bus;
using com.ez.engine.unregister;

namespace com.ez.engine.services.message_bus
{
	public interface IMessageBusService : IInitializable, ISceneLoad
	{
		IMessageBus MessageBus { get; }

		IUnRegister Register<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

		void UnRegister<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

		void Dispatch<TMessage>() where TMessage : IMessage, new();

		void Dispatch<TMessage>(TMessage message) where TMessage : IMessage;

		void Clear();
	}
}
