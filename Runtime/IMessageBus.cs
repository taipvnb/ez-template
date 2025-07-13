using System;
using com.ez.engine.unregister;

namespace com.ez.engine.message_bus
{
    public interface IMessageBus
    {
        IUnRegister Register<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

        void UnRegister<TMessage>(Action<TMessage> listener) where TMessage : IMessage;

        void Dispatch<T>() where T : new();

        void Dispatch<T>(T type);

		void Clear();
	}
}