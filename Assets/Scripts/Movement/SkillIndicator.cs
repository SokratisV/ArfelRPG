using UnityEngine;

namespace RPG.Movement
{
	public class SkillIndicator : MonoBehaviour
	{
		[SerializeField] private GameObject projectorPrefab;

		private Transform _parent;
		private Projector _projector;

		private void Awake()
		{
			_parent = Instantiate(projectorPrefab).transform;
			_projector = _parent.GetComponentInChildren<Projector>();
			_projector.enabled = false;
		}

		public void ShowIndicator(float radius)
		{
			var sizeAdjustment = radius * 3 / 100;
			_projector.orthographicSize = radius + sizeAdjustment;
			_projector.enabled = true;
		}

		public void UpdateIndicator(Vector3 position)
		{
			_parent.position = position;
		}

		public void HideIndicator() => _projector.enabled = false;
	}
}