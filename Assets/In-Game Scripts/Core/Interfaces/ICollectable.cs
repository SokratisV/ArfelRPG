using UnityEngine;

namespace RPG.Control
{
	public interface ICollectable
	{
		Transform GetTransform();
		void Collect();
		float InteractionDistance();
	}
}