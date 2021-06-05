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
	public class ManaUi : MonoBehaviour
	{
		[SerializeField] private float animationTriggerTime = 5f, animationDuration = 2f;
		[SerializeField] private Image manaImage, manaAnimationImage;
		[SerializeField] private Gradient manaBarColor;
		[SerializeField] private TextMeshProUGUI manaText;
		[SerializeField] private GameObject percentText;

		private Coroutine _animationRoutine;
		private Mana _playerMana;
		private float _animationTimer;

		private void Awake() => _playerMana = PlayerFinder.Player.GetComponent<Mana>();
		private void Start() => CalculateManaFill(0);

		private void OnEnable() => _playerMana.OnManaChange += CalculateManaFill;
		private void OnDisable() => _playerMana.OnManaChange -= CalculateManaFill;

		private void CalculateManaFill(float _)
		{
			var manaFraction = _playerMana.GetFraction();
			manaImage.fillAmount = manaFraction;
			manaImage.color = manaBarColor.Evaluate(manaFraction);
			DisableText();
			manaText.SetText($"{manaFraction * 100:N0}");
			_animationRoutine = _animationRoutine.StartCoroutine(this, AnimationCoroutine());
		}

		private void DisableText()
		{
			var toggle = manaImage.fillAmount >= .95f;
			percentText.SetActive(!toggle);
			manaText.gameObject.SetActive(!toggle);
		}

		private IEnumerator AnimationCoroutine()
		{
			_animationTimer = animationTriggerTime;
			while (_animationTimer >= 0)
			{
				_animationTimer -= Time.deltaTime;
				yield return null;
			}

			var currentFill = manaAnimationImage.fillAmount;
			var targetFill = manaImage.fillAmount;
			if (targetFill >= currentFill)
			{
				manaAnimationImage.fillAmount = targetFill;
				yield break;
			}

			LeanTween.value(gameObject, currentFill, targetFill, animationDuration)
				.setEaseLinear()
				.setOnUpdate(value => manaAnimationImage.fillAmount = value);
		}
	}
}