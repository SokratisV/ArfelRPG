using UnityEngine;

namespace RPG.Combat
{
	public class AggroGroup : MonoBehaviour
	{
		[SerializeField] private Fighter[] fighters;
		[SerializeField] private bool activateOnStart;

		private void Start() => Activate(activateOnStart);

		public void Activate(bool shouldActivate)
		{
			foreach(var fighter in fighters)
			{
				fighter.enabled = shouldActivate;
				if(fighter.TryGetComponent<CombatTarget>(out var combatTarget))
				{
					combatTarget.enabled = shouldActivate;
				}
			}
		}
	}
}