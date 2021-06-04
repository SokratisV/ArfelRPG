using System;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
	public class Mana : MonoBehaviour
	{
		public event Action<float> OnManaChange;

		public float CurrentMana => _mana;
		public float MaxMana => maxMana;

		[SerializeField] private float maxMana = 200;
		[SerializeField] private float manaRegen = 5;

		private float _mana;
		private BaseStats _baseStats;

		#region Unity

		private void Awake()
		{
			_mana = maxMana;
			_baseStats = GetComponent<BaseStats>();
		}

		private void Update()
		{
			if (_mana < maxMana)
			{
				RestoreMana(manaRegen * Time.deltaTime);
			}
		}

		private void OnEnable() => _baseStats.OnLevelUp += RestoreAllMana;

		private void OnDisable() => _baseStats.OnLevelUp -= RestoreAllMana;

		#endregion

		#region Public

		public bool UseMana(float manaToUse)
		{
			if (manaToUse > _mana)
			{
				return false;
			}

			_mana -= manaToUse;
			_mana = Mathf.Clamp(_mana, 0, maxMana);
			OnManaChange?.Invoke(manaToUse);
			return true;
		}

		public void RestoreMana(float manaToRestore)
		{
			_mana += manaToRestore;
			_mana = Mathf.Clamp(_mana, 0, maxMana);
			OnManaChange?.Invoke(manaToRestore);
		}

		#endregion

		#region Private

		private void RestoreAllMana() => _mana = maxMana;
		
		[ContextMenu("Use 30 mana")]
		public void ManaUseTest() => UseMana(30);

		[ContextMenu("Restore 40 mana")]
		public void ManRestoreTest() => RestoreMana(40);

		#endregion

		public float GetFraction() => _mana / maxMana;
	}
}