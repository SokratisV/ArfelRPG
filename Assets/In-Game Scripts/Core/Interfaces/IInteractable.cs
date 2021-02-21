using UnityEngine;

namespace RPG.Core
{
	public interface IInteractable
	{
		Transform GetTransform();
		void Interact();
		float InteractionDistance();
	}
}