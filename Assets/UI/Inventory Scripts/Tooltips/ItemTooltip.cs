using UnityEngine;
using TMPro;
using RPG.Inventories;

namespace RPG.UI.Inventories
{
	/// <summary>
	/// Root of the tooltip prefab to expose properties to other classes.
	/// </summary>
	public class ItemTooltip : MonoBehaviour
	{
		// CONFIG DATA
		[SerializeField] private TextMeshProUGUI titleText = null;
		[SerializeField] private TextMeshProUGUI bodyText = null;

		public void Setup(InventoryItem item)
		{
			titleText.text = item.GetDisplayName;
			bodyText.text = item.GetDescription;
		}
	}
}