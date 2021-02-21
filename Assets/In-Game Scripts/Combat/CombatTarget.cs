using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
	[RequireComponent(typeof(Health))]
	public class CombatTarget : MonoBehaviour, IRaycastable
	{
		private OutlineableComponent _outlineableComponent;

		private void Awake() => _outlineableComponent = new OutlineableComponent(gameObject);

		public CursorType GetCursorType() => CursorType.Combat;
		
		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public bool HandleRaycast(GameObject player)
		{
			if(!enabled) return false;
			var fighter = player.GetComponent<Fighter>();
			if(fighter == null && !fighter.CanAttack(gameObject)) return false;
			CheckPressedButtons(fighter);
			return true;
		}

		private void CheckPressedButtons(Fighter fighter)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(Input.GetMouseButtonDown(0))
				{
					fighter.QueueAttackAction(gameObject);
				}
			}
			else
			{
				if(Input.GetMouseButtonDown(0))
				{
					fighter.StartAttackAction(gameObject);
				}
			}
		}

		public float InteractionDistance() => 0f;
	}
}