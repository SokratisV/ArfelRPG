using RPG.Attributes;
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
			_health = GameObject.FindWithTag("Player").GetComponent<Health>();
			_text = GetComponent<TextMeshProUGUI>();
		}

		private void Update() => _text.SetText($"{_health.GetHealthPoints():0}/{_health.GetMaxHealthPoints():0}");
	}
}