using Cysharp.Threading.Tasks;

namespace com.ez.engine.manager.ui
{
	public interface IModalBackdropHandler
	{
		UniTask BeforeModalEnter(ModalView modalView, ModalViewConfig modalConfig, int modalIndex, bool playAnimation);

		void AfterModalEnter(ModalView modal, bool playAnimation);

		UniTask BeforeModalExit(ModalView modal, bool playAnimation);

		void AfterModalExit(ModalView modal, bool playAnimation);
	}
}
