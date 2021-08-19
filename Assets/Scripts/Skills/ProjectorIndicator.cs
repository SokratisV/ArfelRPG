using UnityEngine;

namespace RPG.Skills
{
	public class ProjectorIndicator : ISkillIndicator
	{
		private readonly Transform _parent;
		private readonly Projector _projector;

		public ProjectorIndicator(GameObject parentObject)
		{
			_parent = parentObject.transform;
			_projector = _parent.GetComponentInChildren<Projector>();
			_projector.enabled = false;
		}

		public void ShowIndicator(float radius)
		{
			var sizeAdjustment = radius * 3 / 100;
			_projector.orthographicSize = radius + sizeAdjustment;
			_projector.enabled = true;
		}

		public void UpdateIndicator(Vector3 position) => _parent.position = position;

		public void HideIndicator() => _projector.enabled = false;
	}
}