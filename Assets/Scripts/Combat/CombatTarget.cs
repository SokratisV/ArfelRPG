using RPG.Attributes;
using RPG.Core;
using RPG.Skills;
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
			var skillUser = player.GetComponent<SkillUser>();
			if(skillUser == null && !skillUser.CanCast(gameObject)) return false;
			CheckPressedButtons(skillUser);
			return true;
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public bool HandleRaycast(GameObject player)
		{
			if(!enabled) return false;
			var fighter = player.GetComponent<Fighter>();
			if(fighter == null && !fighter.CanAttack(gameObject)) return false;
			CheckPressedButtons(fighter);
			return true;
		}

		private void CheckPressedButtons(Component component)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(Input.GetMouseButtonDown(0))
				{
					if(component is Fighter fighter) fighter.QueueAttackAction(gameObject);
					if(component is SkillUser skillUser) skillUser.UseSelectedSkill(gameObject);
				}
			}
			else
			{
				if(Input.GetMouseButtonDown(0))
				{
					if(component is Fighter fighter) fighter.StartAttackAction(gameObject);
					if(component is SkillUser skillUser) skillUser.UseSelectedSkill(gameObject);
				}
			}
		}

		public float InteractionDistance() => 0f;
	}
}