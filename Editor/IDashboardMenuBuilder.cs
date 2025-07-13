using Sirenix.OdinInspector.Editor;

namespace com.ez.engine.dashboard.editor
{
	public interface IDashboardMenuBuilder
	{
		int Order { get; }

		void Build(OdinMenuTree tree);
	}
}
