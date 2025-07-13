using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace com.ez.engine.dashboard.editor
{
	public class DashboardWindow : OdinMenuEditorWindow
	{
		private static DashboardEditor _dashboardEditor;

		[MenuItem("EzEngine/Project Dashboard", false, -9999)]
		public static void ShowWindow()
		{
			GetWindow(typeof(DashboardWindow), false, "Project Dashboard");
		}

		protected override OdinMenuTree BuildMenuTree()
		{
			if (_dashboardEditor == null)
			{
				_dashboardEditor = new DashboardEditor();
				_dashboardEditor.RegisterRebuildDashboardAction(ForceMenuTreeRebuild);
				_dashboardEditor.RefreshMenuBuilders();
			}

			var tree = new OdinMenuTree
			{
				Config =
				{
					DrawSearchToolbar = true
				},
				Selection =
				{
					SupportsMultiSelect = false
				}
			};

			tree.Add("Home", _dashboardEditor, SdfIconType.HouseFill);

			foreach (var builder in _dashboardEditor.MenuBuilders)
			{
				builder.Build(tree);
			}

			return tree;
		}
	}
}
