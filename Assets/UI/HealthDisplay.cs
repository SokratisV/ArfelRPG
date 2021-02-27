using RPG.Attributes;
using RPG.Core;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
	public class HealthDisplay : MonoBehaviour
	{
		private Health _health;
		private TextMeshProUGUI _text;

		private void Awake()
		{
			_health = PlayerFinder.Player.GetComponent<Health>();
			_text = GetComponent<TextMeshProUGUI>();
		}

		private void Update() => _text.SetText($"{_health.GetHealthPoints():0}/{_health.GetMaxHealthPoints():0}");
	}
}