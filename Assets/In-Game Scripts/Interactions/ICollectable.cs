using UnityEngine;

namespace RPG.Core
{
	public interface ICollectable
	{
		Transform GetTransform();
		void Collect();
		float InteractionDistance();
	}
}