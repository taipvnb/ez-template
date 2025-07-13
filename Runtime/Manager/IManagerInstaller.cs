using System;
using System.Collections.Generic;

namespace com.ez.engine.core.manager
{
	public interface IManagerInstaller
	{
		IEnumerable<IManager> Managers { get; }

		IManagerInstaller Binding<TManager>(TManager instance) where TManager : IManager;

		IManagerInstaller Binding<TManager>(Type type, TManager instance) where TManager : IManager;

		TManager GetManager<TManager>() where TManager : IManager;

		void Clear();
	}
}
