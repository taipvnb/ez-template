using System;

namespace com.ez.engine.core.di
{
	internal sealed class PropertyInjectorException : Exception
	{
		public PropertyInjectorException(Exception e) : base(e.Message) { }
	}
}
