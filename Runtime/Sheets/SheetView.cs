using UnityEngine;

namespace com.ez.engine.manager.ui
{
	[DisallowMultipleComponent]
	public class SheetView : ControlView
	{
		[SerializeField] private int _renderingOrder;

		protected sealed override void OnAfterLoad()
		{
			RectTransform.FillParent(Parent);

			var siblingIndex = 0;
			for (var i = 0; i < Parent.childCount; i++)
			{
				var child = Parent.GetChild(i);
				var childControl = child.GetComponent<SheetView>();
				siblingIndex = i;

				if (_renderingOrder >= childControl._renderingOrder)
				{
					continue;
				}

				break;
			}

			RectTransform.SetSiblingIndex(siblingIndex);
		}

		protected sealed override void OnBeforeEnter()
		{
			SetTransitionProgress(0.0f);
			Alpha = 0.0f;
			RectTransform.FillParent(Parent);
		}

		protected override void OnEnter()
		{
			Alpha = 1.0f;
		}

		protected sealed override void OnBeforeExit()
		{
			SetTransitionProgress(0.0f);
			Alpha = 1.0f;
			RectTransform.FillParent(Parent);
		}

		protected override void OnExit()
		{
			Alpha = 0.0f;
			SetTransitionProgress(1.0f);
		}

		protected sealed override ITransitionAnimation GetAnimation(bool enter, ControlView partner)
		{
			var partnerIdentifier = partner == true ? partner.Identifier : string.Empty;
			var anim = AnimationContainer.GetAnimation(enter, partnerIdentifier);

			if (anim == null)
			{
				return Settings.GetDefaultSheetTransitionAnimation(enter);
			}

			return anim;
		}
	}
}
