using RPG.Core;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
	public class ExperienceDisplay : MonoBehaviour
	{
		private Experience _experience;
		private TextMeshProUGUI _text;

		private void Awake()
		{
			_experience = PlayerFinder.Player.GetComponent<Experience>();
			_text = GetComponent<TextMeshProUGUI>();
		}

		private void Update() => _text.SetText($"{_experience.GetPoints():0}");
	}
}