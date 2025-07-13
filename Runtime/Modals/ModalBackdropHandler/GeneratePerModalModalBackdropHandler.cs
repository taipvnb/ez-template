using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ez.engine.manager.ui
{
	public class GeneratePerModalModalBackdropHandler : IModalBackdropHandler
	{
		private readonly ModalBackdrop _backdrop;

		public GeneratePerModalModalBackdropHandler(ModalBackdrop backdrop)
		{
			_backdrop = backdrop;
		}

		public UniTask BeforeModalEnter(ModalView modalView, ModalViewConfig modalConfig, int modalIndex, bool playAnimation)
		{
			var parent = (RectTransform)modalView.transform.parent;
			_backdrop.Setup(parent, modalConfig.BackdropAlpha, modalConfig.CloseWhenClickOnBackdrop);
			var backdropSiblingIndex = modalIndex * 2;
			_backdrop.transform.SetSiblingIndex(backdropSiblingIndex);
			return _backdrop.EnterAsync(playAnimation);
		}

		public void AfterModalEnter(ModalView modal, bool playAnimation) { }

		public UniTask BeforeModalExit(ModalView modal, bool playAnimation)
		{
			return _backdrop.ExitAsync(playAnimation);
		}

		public void AfterModalExit(ModalView modal, bool playAnimation)
		{
			Object.Destroy(_backdrop.gameObject);
		}
	}
}
