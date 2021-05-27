using System.Collections;
using System.Globalization;
using RPG.Attributes;
using RPG.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class HealthUi : MonoBehaviour
	{
		[SerializeField] private float animationTriggerTime = 5f, animationDuration = 2f;
		[SerializeField] private Image healthImage, healthAnimationImage;
		[SerializeField] private Gradient healthBarColor;
		[SerializeField] private TextMeshProUGUI healthText;

		private Coroutine _animationRoutine;
		private Health _playerHealth;
		private float _animationTimer;
		private float _initialAlpha;

		private void Awake()
		{
			_playerHealth = PlayerFinder.Player.GetComponent<Health>();
			_initialAlpha = healthImage.color.a;
			AdjustAlpha();
		}

		private void OnEnable() => _playerHealth.OnHealthChange += CalculateHealthFill;

		private void OnDisable() => _playerHealth.OnHealthChange -= CalculateHealthFill;

		private void CalculateHealthFill(float _)
		{
			var healthFraction = _playerHealth.GetFraction();
			healthImage.fillAmount = healthFraction;
			AdjustAlpha();
			healthImage.color = healthBarColor.Evaluate(healthFraction);
			healthText.SetText((healthFraction * 100).ToString(CultureInfo.InvariantCulture));
			_animationRoutine = _animationRoutine.StartCoroutine(this, AnimationCoroutine());
		}

		private void AdjustAlpha()
		{
			if (healthImage.fillAmount >= .95f)
			{
				var targetColor = healthImage.color;
				targetColor.a = _initialAlpha * .5f;
				healthImage.color = targetColor;
			}
			else
			{
				var targetColor = healthImage.color;
				targetColor.a = _initialAlpha;
				healthImage.color = targetColor;
			}
		}

		private IEnumerator AnimationCoroutine()
		{
			_animationTimer = animationTriggerTime;
			while (_animationTimer >= 0)
			{
				_animationTimer -= Time.deltaTime;
				yield return null;
			}

			var currentFill = healthAnimationImage.fillAmount;
			var targetFill = healthImage.fillAmount;
			if (targetFill >= currentFill)
			{
				healthAnimationImage.fillAmount = targetFill;
				yield break;
			}

			LeanTween.value(gameObject, currentFill, targetFill, animationDuration)
				.setEaseLinear()
				.setOnUpdate(value => healthAnimationImage.fillAmount = value);
		}
	}
}