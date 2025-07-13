using Cysharp.Threading.Tasks;

namespace com.ez.engine.manager.ui
{
	public interface IScreenLifecycleEvent : IViewLifecycleEvent
	{
		UniTask WillPushEnter();

		void DidPushEnter();

		UniTask WillPushExit();

		void DidPushExit();

		UniTask WillPopEnter();

		void DidPopEnter();

		UniTask WillPopExit();

		void DidPopExit();
	}
}
