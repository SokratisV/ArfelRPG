using TMPro;
using UnityEngine;

namespace UI.Scripts.Attributes
{
	public class AttributeTooltipUi : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI bodyText = null;

		public void Setup(string description) => bodyText.text = description;
	}
}