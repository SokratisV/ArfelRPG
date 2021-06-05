using System;
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
		[SerializeField] private GameObject percentText;

		private Coroutine _animationRoutine;
		private Health _playerHealth;
		private float _animationTimer;

		private void Awake() => _playerHealth = PlayerFinder.Player.GetComponent<Health>();
		private void Start() => CalculateHealthFill(0);

		private void OnEnable() => _playerHealth.OnHealthChange += CalculateHealthFill;
		private void OnDisable() => _playerHealth.OnHealthChange -= CalculateHealthFill;

		private void CalculateHealthFill(float _)
		{
			var healthFraction = _playerHealth.GetFraction();
			healthImage.fillAmount = healthFraction;
			healthImage.color = healthBarColor.Evaluate(healthFraction);
			DisableText();
			healthText.SetText($"{healthFraction * 100:N0}");
			_animationRoutine = _animationRoutine.StartCoroutine(this, AnimationCoroutine());
		}

		private void DisableText()
		{
			var toggle = healthImage.fillAmount >= .95f;
			percentText.SetActive(!toggle);
			healthText.gameObject.SetActive(!toggle);
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