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

		public void ShowIndicator(Vector3 position, float radius)
		{
			var sizeAdjustment = radius * 3 / 100;
			_projector.orthographicSize = radius + sizeAdjustment;
			_parent.position = position;
			_projector.enabled = true;
		}

		public void HideIndicator() => _projector.enabled = false;
	}
}