using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ez.engine.manager.ui
{
	public class PopButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField] private Vector3 _clickDownScale = new(0.95f, 0.95f, 0.95f);
		[SerializeField] private Vector3 _normalScale = Vector3.one;

		public void OnPointerDown(PointerEventData eventData)
		{
			transform.localScale = _clickDownScale;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			transform.localScale = _normalScale;
		}
	}
}
