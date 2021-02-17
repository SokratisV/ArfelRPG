using RPG.Control;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
	[RequireComponent(typeof(Health))]
	public class CombatTarget : MonoBehaviour, IRaycastable
	{
		private OutlineableComponent _outlineableComponent;

		private void Awake()
		{
			if(TryGetComponent(out Outline outline))
				_outlineableComponent = new OutlineableComponent(outline);
			else
			{
				outline = gameObject.AddComponent<Outline>();
				outline.enabled = false;
				_outlineableComponent = new OutlineableComponent(outline);
			}
		}

		public CursorType GetCursorType() => CursorType.Combat;

		public bool HandleRaycast(PlayerController callingController)
		{
			if(!callingController.GetComponent<Fighter>().CanAttack(gameObject)) return false;
			CheckPressedButtons(callingController);
			return true;
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		private void CheckPressedButtons(PlayerController callingController)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(Input.GetMouseButtonDown(0))
				{
					callingController.GetComponent<Fighter>().QueueAttackAction(gameObject);
				}
			}
			else
			{
				if(Input.GetMouseButtonDown(0))
				{
					callingController.GetComponent<Fighter>().StartAttackAction(gameObject);
				}
			}
		}

		public float GetInteractionRange() => 0f;
	}
}