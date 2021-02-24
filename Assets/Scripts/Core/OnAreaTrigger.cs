using UnityEngine;

namespace RPG.Core
{
	public class OnAreaTrigger : MonoBehaviour
	{
		[SerializeField] private Areas area;
		private AreaEventManager _areaEventManager;

		private void Awake() => _areaEventManager = GetComponentInParent<AreaEventManager>();

		private void OnTriggerEnter(Collider other)
		{
			if(!other.CompareTag("Player")) return;
			_areaEventManager.EnterNewArea(area);
		}

		private void OnTriggerExit(Collider other)
		{
			if(!other.CompareTag("Player")) return;
			_areaEventManager.ExitArea(area);
		}
	}
}