using Core.Interfaces;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
	public class FriendlyTarget : MonoBehaviour, ISkillcastable
	{
		private OutlineableComponent _outlineableComponent;

		private void Awake() => _outlineableComponent = new OutlineableComponent(gameObject, GlobalValues.EnemyColor);

		public CursorType GetSkillCursorType() => CursorType.Skill;

		public bool HandleSkillcast(GameObject player)
		{
			if(!enabled) return false;
			if(player.TryGetComponent(out ISpellActionable skillUser))
			{
				if(!skillUser.CanExecute(gameObject)) return false;
				CheckPressedButtons(skillUser);
			}

			return true;
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		private void CheckPressedButtons(IActionable actionable)
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
	}
}