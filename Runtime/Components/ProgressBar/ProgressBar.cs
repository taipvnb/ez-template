using UnityEngine;
using UnityEngine.UI;

namespace com.ez.engine.manager.ui
{
	[AddComponentMenu("UI/Progress Bar")]
	[SelectionBase]
	public sealed class ProgressBar : ProgressBarBase
	{
		private enum FillMode
		{
			FillAmount,
			Stretch
		}

		private enum Direction
		{
			LeftToRight,
			RightToLeft,
			BottomToTop,
			TopToBottom
		}

		private enum Axis
		{
			Horizontal,
			Vertical
		}

		[SerializeField] private FillMode _fillMode;
		[SerializeField] private Image _fillImage;
		[SerializeField] private Direction _direction;
		[SerializeField] private RectTransform _fillRect;

		public RectTransform FillRect
		{
			get => _fillRect;
			set => _fillRect = value;
		}

		public Image FillImage
		{
			get => _fillImage;
			set => _fillImage = value;
		}

		protected override void OnValueChangedCore(float value)
		{
			switch (_fillMode)
			{
				case FillMode.FillAmount:
					_fillImage.fillAmount = GetNormalizedValue();
					break;
				case FillMode.Stretch:
					var zero = Vector2.zero;
					var one = Vector2.one;

					if (_direction == Direction.RightToLeft || _direction == Direction.TopToBottom)
					{
						zero[(int)DirectionToAxis()] = 1f - GetNormalizedValue();
					}
					else
					{
						one[(int)DirectionToAxis()] = GetNormalizedValue();
					}

					_fillRect.anchorMin = zero;
					_fillRect.anchorMax = one;
					break;
			}
		}

		private Axis DirectionToAxis()
		{
			return (_direction != Direction.LeftToRight && _direction != Direction.RightToLeft) ? Axis.Vertical : Axis.Horizontal;
		}
	}
}
