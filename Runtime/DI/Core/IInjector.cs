using System;

namespace com.ez.engine.core.di
{
	public interface IInjector : IDisposable
	{
		void Resolve(object target);

		bool HasBinding(Type type);
		
		void AddSingleton(Type concrete, Type contract);

		void AddSingleton(object instance, Type contract);

		void AddSingleton<T>(Func<Injector, T> factory, Type contract);

		void AddTransient(Type concrete, Type contract);

		void AddTransient(object instance, Type contract);

		void AddTransient<T>(Func<Injector, T> factory, Type contract);
		
		void Remove(Type contract);
	}
}
