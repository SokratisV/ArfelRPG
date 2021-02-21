using RPG.Core;
using UnityEngine;

namespace RPG.Dialogue
{
	public class AIConverser : MonoBehaviour, IRaycastable, IInteractable
	{
		[SerializeField] private Dialogue dialogue;
		private OutlineableComponent _outlineableComponent;
		private PlayerConversant _playerConversant = null;

		private void Awake() => _outlineableComponent = new OutlineableComponent(gameObject);

		public CursorType GetCursorType() => CursorType.Dialogue;

		public bool HandleRaycast(GameObject player)
		{
			if(dialogue == null) return false;
			if(_playerConversant == null)
			{
				_playerConversant = player.GetComponent<PlayerConversant>();
			}

			if(!_playerConversant.CanInteract(this)) return false;
			CheckPressedButtons(_playerConversant);
			return true;
		}

		private void CheckPressedButtons(PlayerConversant playerConversant)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(Input.GetMouseButtonDown(0))
				{
					playerConversant.QueueInteractAction(gameObject);
				}
			}
			else
			{
				if(Input.GetMouseButton(0))
				{
					playerConversant.StartInteractAction(this);
				}
			}
		}

		public void ShowInteractivity() => _outlineableComponent.ShowOutline(this);

		public Transform GetTransform() => transform;

		public void Interact() => _playerConversant.StartDialogue(dialogue);

		public float InteractionDistance() => 2f;
	}
}