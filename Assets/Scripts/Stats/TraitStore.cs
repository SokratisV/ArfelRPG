using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
	public class TraitStore : MonoBehaviour, IModifierProvider
	{
		public event Action<Trait, int> OnStagedPointsChanged;
		[SerializeField] private TraitBonus[] bonusConfig;

		private Dictionary<Trait, int> _assignedPoints = new Dictionary<Trait, int>();
		private Dictionary<Trait, int> _stagedPoints = new Dictionary<Trait, int>();
		private Dictionary<Stat, Dictionary<Trait, float>> _additiveBonusCache;
		private Dictionary<Stat, Dictionary<Trait, float>> _percentageBonusCache;
		private BaseStats _baseStats;

		#region Unity

		private void Awake()
		{
			_baseStats = GetComponent<BaseStats>();
			SetupModifierDictionaries();
		}

		#endregion

		#region Public

		public int UnassignedPoints => GetAssignablePoints() - GetTotalProposedPoints();

		public int GetTotalProposedPoints() => _assignedPoints.Values.Sum() + _stagedPoints.Values.Sum();
		public int GetProposedPoints(Trait trait) => GetPoints(trait) + GetStagedPoints(trait);
		public int GetPoints(Trait trait) => _assignedPoints.ContainsKey(trait) ? _assignedPoints[trait] : 0;
		public int GetStagedPoints(Trait trait) => _stagedPoints.ContainsKey(trait) ? _stagedPoints[trait] : 0;

		public void AssignPoints(Trait trait, int points)
		{
			if (!CanAssignPoints(trait, points)) return;
			_stagedPoints[trait] = GetStagedPoints(trait) + points;
			OnStagedPointsChanged?.Invoke(trait, points);
		}

		public bool CanAssignPoints(Trait trait, int points)
		{
			if (GetStagedPoints(trait) + points < 0) return false;
			return UnassignedPoints >= points;
		}

		public void Commit()
		{
			foreach (var trait in _stagedPoints.Keys)
			{
				_assignedPoints[trait] = GetProposedPoints(trait);
			}

			_stagedPoints.Clear();
			OnStagedPointsChanged?.Invoke(Trait.Constitution, 0);
		}

		public int GetAssignablePoints() => (int) _baseStats.GetStat(Stat.TraitPoints);

		#endregion

		#region Interface

		public IEnumerable<float> GetAdditiveModifiers(Stat stat)
		{
			if (!_additiveBonusCache.ContainsKey(stat)) yield break;
			foreach (var trait in _additiveBonusCache[stat].Keys)
			{
				var bonus = _additiveBonusCache[stat][trait];
				yield return bonus * GetPoints(trait);
			}
		}

		public IEnumerable<float> GetPercentageModifiers(Stat stat)
		{
			if (!_percentageBonusCache.ContainsKey(stat)) yield break;
			foreach (var trait in _percentageBonusCache[stat].Keys)
			{
				var bonus = _percentageBonusCache[stat][trait];
				yield return bonus * GetPoints(trait);
			}
		}

		#endregion

		#region Private

		private void SetupModifierDictionaries()
		{
			_additiveBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
			_percentageBonusCache = new Dictionary<Stat, Dictionary<Trait, float>>();
			foreach (var traitBonus in bonusConfig)
			{
				if (!_additiveBonusCache.ContainsKey(traitBonus.stat))
				{
					_additiveBonusCache[traitBonus.stat] = new Dictionary<Trait, float>();
				}

				if (!_percentageBonusCache.ContainsKey(traitBonus.stat))
				{
					_percentageBonusCache[traitBonus.stat] = new Dictionary<Trait, float>();
				}

				_additiveBonusCache[traitBonus.stat][traitBonus.trait] = traitBonus.additiveBonusPerPoint;
				_percentageBonusCache[traitBonus.stat][traitBonus.trait] = traitBonus.percentageBonusPerPoint;
			}
		}

		#endregion
	}

	[Serializable]
	public class TraitBonus
	{
		public Trait trait;
		public Stat stat;
		public float additiveBonusPerPoint;
		public float percentageBonusPerPoint;
	}
}