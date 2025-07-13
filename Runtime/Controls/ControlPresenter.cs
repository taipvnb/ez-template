namespace com.ez.engine.manager.ui
{
	public class ControlPresenter<TControlView> : ViewPresenter<TControlView> where TControlView : ControlView
	{
		protected override void Initialize(TControlView view) { }

		protected override void Dispose(TControlView view) { }
	}
}
