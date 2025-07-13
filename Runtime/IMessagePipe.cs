using System;
using com.ez.engine.unregister;

namespace com.ez.engine.message_bus
{
    public interface IMessagePipe
    {
        IUnRegister Register(Action listener);
    }
}