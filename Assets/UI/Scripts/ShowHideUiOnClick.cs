using UnityEngine;
using UnityEngine.EventSystems;

public class ShowHideUiOnClick : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		var shouldAct = eventData.button switch
		{
			PointerEventData.InputButton.Right => true,
			_ => false
		};
		if (shouldAct)
		{
			gameObject.SetActive(false);
			foreach (Transform child in transform.parent)
			{
				if (child.gameObject.activeInHierarchy) return;
			}
			transform.parent.gameObject.SetActive(false);
		}
	}
}