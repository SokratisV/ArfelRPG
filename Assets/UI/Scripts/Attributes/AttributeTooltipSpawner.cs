using RPG.Core;
using RPG.Stats;
using RPG.UI.Tooltips;
using UnityEngine;

namespace UI.Scripts.Attributes
{
	[RequireComponent(typeof(AttributeUi))]
	public class AttributeTooltipSpawner : TooltipSpawner
	{
		private TraitStore _playerStats;

		private void Start() => _playerStats = PlayerFinder.Player.GetComponent<TraitStore>();

		public override void UpdateTooltip(GameObject tooltip)
		{
			var description = string.Empty;
			foreach (var traitBonus in _playerStats.BonusesOfTrait(GetComponent<AttributeUi>().Trait))
			{
				description += $"Every point in {traitBonus.trait} increases {traitBonus.stat} by: ";
				var tempValue = traitBonus.additiveBonusPerPoint;
				if (tempValue > 0) description += $"<color=green>{tempValue}</color>.\n";
				tempValue = traitBonus.percentageBonusPerPoint;
				if (tempValue > 0) description += $"<color=green>{tempValue}%</color>.\n";
			}

			tooltip.GetComponent<AttributeTooltipUi>().Setup(description);
		}

		public override bool CanCreateTooltip() => true;
	}
}