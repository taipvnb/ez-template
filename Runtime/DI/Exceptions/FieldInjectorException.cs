using System;

namespace com.ez.engine.core.di
{
	internal sealed class FieldInjectorException : Exception
	{
		public FieldInjectorException(Exception e) : base(e.Message) { }
	}
}
