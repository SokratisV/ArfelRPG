using RPG.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
	public class ExperienceDisplay : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI currentXpText, nextLevelXpText, levelText;
		[SerializeField] private Image xpBar;

		private Experience _experience;
		private BaseStats _baseStats;
		private float _currentLevelProgress, _nextLevelExperience;

		private void Start()
		{
			_experience = PlayerFinder.Player.GetComponent<Experience>();
			_baseStats = PlayerFinder.Player.GetComponent<BaseStats>();
			_experience.OnExperienceGained += UpdateOnExperienceGain;
			_baseStats.OnLevelUp += UpdateOnLevel;
			InitialSetup();
		}

		private void InitialSetup()
		{
			var level = _baseStats.GetLevel();
			if (level == 1)
			{
				_currentLevelProgress = _experience.GetPoints();
				_nextLevelExperience = _baseStats.GetCurrentLevelExperience();
				_currentLevelProgress = Mathf.Clamp(_currentLevelProgress, 0, _nextLevelExperience);
			}
			else
			{
				_currentLevelProgress = _experience.GetPoints() - _baseStats.GetPreviousLevelExperience();
				_nextLevelExperience = _baseStats.GetCurrentLevelExperience() - _baseStats.GetPreviousLevelExperience();
				_currentLevelProgress = Mathf.Clamp(_currentLevelProgress, 0, _nextLevelExperience);
			}

			nextLevelXpText.SetText($"{_nextLevelExperience:0}");
			levelText.SetText(level.ToString());
			currentXpText.SetText($"{_currentLevelProgress:0}");
			xpBar.fillAmount = _currentLevelProgress / _nextLevelExperience;
		}

		private void OnDestroy()
		{
			_experience.OnExperienceGained -= UpdateOnExperienceGain;
			_baseStats.OnLevelUp -= UpdateOnLevel;
		}

		private void UpdateOnLevel()
		{
			_currentLevelProgress = _currentLevelProgress = _experience.GetPoints() - _baseStats.GetPreviousLevelExperience();
			_nextLevelExperience = _baseStats.GetCurrentLevelExperience() - _baseStats.GetPreviousLevelExperience();
			nextLevelXpText.SetText($"{_nextLevelExperience:0}");
			levelText.SetText(_baseStats.GetLevel().ToString());
			xpBar.fillAmount = _currentLevelProgress / _nextLevelExperience;
		}

		private void UpdateOnExperienceGain(float xpGained)
		{
			if (_baseStats.GetLevel() == 1)
			{
				_currentLevelProgress = _experience.GetPoints();
			}
			else
			{
				_currentLevelProgress = _experience.GetPoints() - _baseStats.GetPreviousLevelExperience();
				_currentLevelProgress = Mathf.Clamp(_currentLevelProgress, 0, _nextLevelExperience);
			}

			currentXpText.SetText($"{_currentLevelProgress:0}");
			xpBar.fillAmount = _currentLevelProgress / _nextLevelExperience;
		}
	}
}