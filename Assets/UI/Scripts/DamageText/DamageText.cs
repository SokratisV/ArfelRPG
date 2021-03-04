using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
	public class DamageText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI damageText = null;

		//Animation Event
		public void DestroyText() => Destroy(gameObject);

		public void SetValue(float amount) => damageText.text = $"{amount:0}";
	}
}