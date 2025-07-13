using UnityEngine;
using UnityEngine.Events;

namespace com.ez.engine.manager.ui
{
	public abstract class ProgressBarBase : MonoBehaviour
	{
		[SerializeField] private float _minValue = 0f;
		[SerializeField] private float _maxValue = 1f;
		[SerializeField] private float _value = 0f;
		[SerializeField] private UnityEvent<float> _onValueChanged;

		public float MinValue
		{
			get => _minValue;
			set
			{
				_minValue = value;
				if (Value < _minValue)
				{
					Value = _minValue;
				}
			}
		}

		public float MaxValue
		{
			get => _maxValue;
			set
			{
				_maxValue = value;
				if (Value > _maxValue)
				{
					Value = _maxValue;
				}
			}
		}

		public float Value
		{
			get => _value;
			set => SetValueCore(value);
		}

		public UnityEvent<float> OnValueChanged => _onValueChanged;

		protected virtual void OnValueChangingCore(ref float value) { }
		protected virtual void OnValueChangedCore(float value) { }

		public void SetValueWithoutNotify(float value)
		{
			OnValueChangingCore(ref value);
			_value = value;
			OnValueChangedCore(value);
		}

		public void ForceNotify()
		{
			SetValueCore(_value);
		}

		private void SetValueCore(float value)
		{
			OnValueChangingCore(ref value);
			_value = value;
			OnValueChangedCore(value);

			_onValueChanged.Invoke(value);
		}

		protected float GetNormalizedValue()
		{
			return Mathf.InverseLerp(MinValue, MaxValue, _value);
		}
	}
}
