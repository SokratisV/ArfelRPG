using UnityEngine;

namespace RPG.Skills
{
	public class SkillIndicatorQuad : ISkillIndicator
	{
		private readonly Transform _parent;

		public SkillIndicatorQuad(GameObject parentObject)
		{
			_parent = parentObject.transform;
			_parent.gameObject.SetActive(false);
		}

		public void ShowIndicator(float radius)
		{
			_parent.gameObject.SetActive(true);
			_parent.localScale = new Vector3(radius * 2, 1, radius * 2);
		}

		public void UpdateIndicator(Vector3 position) => _parent.position = position;

		public void HideIndicator() => _parent.gameObject.SetActive(false);
	}
}