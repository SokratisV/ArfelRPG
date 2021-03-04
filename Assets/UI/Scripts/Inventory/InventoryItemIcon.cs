using UnityEngine;
using UnityEngine.UI;
using RPG.Inventories;
using TMPro;

namespace RPG.UI.Inventories
{
	/// <summary>
	/// To be put on the icon representing an inventory item. Allows the slot to
	/// update the icon and number.
	/// </summary>
	[RequireComponent(typeof(Image))]
	public class InventoryItemIcon : MonoBehaviour
	{
		[SerializeField] private GameObject textContainer = null;
		[SerializeField] private TextMeshProUGUI itemNumber = null;

		public void SetItem(InventoryItem item) => SetItem(item, 0);

		public void SetItem(InventoryItem item, int number)
		{
			var iconImage = GetComponent<Image>();
			if(item == null)
			{
				iconImage.enabled = false;
			}
			else
			{
				iconImage.enabled = true;
				iconImage.sprite = item.Icon;
			}

			if(itemNumber)
			{
				if(number <= 1)
				{
					textContainer.SetActive(false);
				}
				else
				{
					textContainer.SetActive(true);
					itemNumber.text = number.ToString();
				}
			}
		}
	}
}