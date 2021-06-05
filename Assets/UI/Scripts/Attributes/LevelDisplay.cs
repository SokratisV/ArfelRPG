using RPG.Core;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
	public class LevelDisplay : MonoBehaviour
	{
		private BaseStats _stats;
		private TextMeshProUGUI _text;

		private void Awake()
		{
			_stats = PlayerFinder.Player.GetComponent<BaseStats>();
			_text = GetComponent<TextMeshProUGUI>();
		}

		private void Update() => _text.SetText($"{_stats.CalculateLevel():0}");
	}
}