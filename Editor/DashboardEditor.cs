using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace com.ez.engine.dashboard.editor
{
	[Serializable]
	[HorizontalGroup("Split", 55)]
	[InfoBox("Made by Dev Team", InfoMessageType.None, Icon = SdfIconType.Terminal)]
	public class DashboardEditor
	{
		public IEnumerable<IDashboardMenuBuilder> MenuBuilders { get; private set; } = Enumerable.Empty<IDashboardMenuBuilder>();

		private Action _rebuildDashboardAction;

		public void RegisterRebuildDashboardAction(Action action)
		{
			_rebuildDashboardAction = action;
		}

		public void RefreshMenuBuilders()
		{
			MenuBuilders = CreateMenuBuilders<IDashboardMenuBuilder>();
			MenuBuilders = MenuBuilders.OrderBy(x => x.Order);
		}

		[Button(ButtonSizes.Large, ButtonStyle.Box)]
		private void RebuildDashboard()
		{
			_rebuildDashboardAction?.Invoke();
		}

		private static IEnumerable<TInterface> CreateMenuBuilders<TInterface>()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var builders = assemblies
				.SelectMany(assembly => assembly.GetTypes())
				.Where(t => t.IsClass && typeof(TInterface).IsAssignableFrom(t))
				.ToArray();

			var objects = new List<TInterface>(builders.Length);
			objects.AddRange(builders.Select(builder => (TInterface)Activator.CreateInstance(builder)));
			return objects;
		}
	}
}
