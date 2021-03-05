using Core.Interfaces;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
	[RequireComponent(typeof(Health))]
	public class CombatTarget : MonoBehaviour, IRaycastable, ISkillcastable
	{
		private OutlineableComponent _outlineableComponent;

		private void Awake() => _outlineableComponent = new OutlineableComponent(gameObject, GlobalValues.EnemyColor);

		public CursorType GetCursorType() => CursorType.Combat;

		public CursorType GetSkillCursorType() => CursorType.Skill;

		public bool HandleSkillcast(GameObject player)
		{
			if(!enabled) return false;
			if(player.TryGetComponent(out ICombatActionable skillUser))
			{
				if(!skillUser.CanExecute(gameObject)) return false;
				CheckPressedButtons(skillUser);
			}

			return true;
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public bool HandleRaycast(GameObject player)
		{
			if(!enabled) return false;
			if(player.TryGetComponent(out ICombatActionable fighter))
			{
				if(!fighter.CanExecute(gameObject)) return false;
				CheckPressedButtons(fighter);
			}

			return true;
		}

		private void CheckPressedButtons(ICombatActionable actionable)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(Input.GetMouseButtonDown(0))
				{
					actionable.QueueExecution(gameObject);
				}
			}
			else
			{
				if(Input.GetMouseButtonDown(0))
				{
					actionable.Execute(gameObject);
				}
			}
		}

		public float InteractionDistance() => 0f;
	}
}