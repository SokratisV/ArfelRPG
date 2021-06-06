using System;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Attributes
{
	public class Mana : MonoBehaviour, ISaveable
	{
		public event Action<float> OnManaChange;

		private LazyValue<float> _mana;
		private BaseStats _baseStats;

		#region Unity

		private void Awake()
		{
			_mana = new LazyValue<float>(GetMaxMana);
			_baseStats = GetComponent<BaseStats>();
		}

		private void Update()
		{
			if (_mana.Value < GetMaxMana())
			{
				RestoreMana(GetManaRegen() * Time.deltaTime);
			}
		}

		private void OnEnable() => _baseStats.OnLevelUp += RestoreAllMana;
		private void OnDisable() => _baseStats.OnLevelUp -= RestoreAllMana;

		#endregion

		#region Public

		public float GetFraction() => _mana.Value / GetMaxMana();
		public float GetMaxMana() => _baseStats.GetStat(Stat.Mana);
		public float GetManaRegen() => _baseStats.GetStat(Stat.ManaRegen);

		public bool UseMana(float manaToUse)
		{
			if (manaToUse > _mana.Value)
			{
				return false;
			}

			_mana.Value -= manaToUse;
			_mana.Value = Mathf.Clamp(_mana.Value, 0, GetMaxMana());
			OnManaChange?.Invoke(manaToUse);
			return true;
		}

		public void RestoreMana(float manaToRestore)
		{
			_mana.Value += manaToRestore;
			_mana.Value = Mathf.Clamp(_mana.Value, 0, GetMaxMana());
			OnManaChange?.Invoke(manaToRestore);
		}

		#endregion

		#region Interface

		public object CaptureState() => _mana.Value;

		public void RestoreState(object state) => _mana.Value = (float) state;

		#endregion

		#region Private

		private void RestoreAllMana() => RestoreMana(GetMaxMana());

		[ContextMenu("Use 30 mana")]
		public void ManaUseTest() => UseMana(30);

		[ContextMenu("Restore 40 mana")]
		public void ManRestoreTest() => RestoreMana(40);

		#endregion
	}
}