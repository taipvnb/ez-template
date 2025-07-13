using UnityEngine;

namespace com.ez.engine.manager.ui
{
	public class TabItem : MonoBehaviour
	{
		[SerializeField] private int _tabIndex;
		[SerializeField] private GameObject _tabActive;
		[SerializeField] private GameObject _tabUnActive;

		public void SetActive(int index)
		{
			_tabActive.SetActive(_tabIndex == index);
			_tabUnActive.SetActive(_tabIndex != index);
		}
	}
}
