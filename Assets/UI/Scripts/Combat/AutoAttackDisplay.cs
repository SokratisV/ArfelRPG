using RPG.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Combat
{
	public class AutoAttackDisplay : MonoBehaviour
	{
		[SerializeField] private Slider slider;
		[SerializeField] private Fighter fighter;

		private void Start()
		{
			slider.maxValue = fighter.AttackSpeed;
			fighter.OnWeaponChanged += AdjustSlider;
		}

		private void AdjustSlider(WeaponConfig obj) => slider.maxValue = obj.AttackSpeed;

		private void Update() => slider.value = slider.maxValue - fighter.TimeSinceLastAttack;
	}
}