using Cysharp.Threading.Tasks;

namespace com.ez.engine.manager.ui
{
	public interface IViewLifecycleEvent
	{
		UniTask Initialize();

		UniTask WillEnter();

		void DidEnter();

		UniTask WillExit();

		void DidExit();

		UniTask Cleanup();
	}
}
