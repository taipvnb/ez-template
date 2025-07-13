using System;
using System.Reflection;

namespace com.ez.engine.core.di
{
	internal interface IActivatorFactory
	{
		ObjectActivator GenerateActivator(Type type, ConstructorInfo constructor, Type[] parameters);
		ObjectActivator GenerateDefaultActivator(Type type);
	}
}
