using RPG.Core.SystemEvents;
using TMPro;
using UnityEngine;

namespace RPG.UI.Tooltips
{
	public class WorldObjectTooltip : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI titleText = null;
		[SerializeField] private TextMeshProUGUI bodyText = null;

		public void Setup(WorldObjectTooltipData data)
		{
			titleText.text = data.objectName;
			bodyText.text = data.objectInfo;
		}
	}
}