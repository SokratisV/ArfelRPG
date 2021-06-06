using RPG.Core;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class TraitUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI unassignedPoints;
		[SerializeField] private Button commitButton;

		private TraitStore _traitStore;
		private bool _isUpdating;

		private void Start()
		{
			_traitStore = PlayerFinder.Player.GetComponent<TraitStore>();
			commitButton.onClick.AddListener(_traitStore.Commit);
		}

		private void Update()
		{
			if (_isUpdating) unassignedPoints.SetText(_traitStore.UnassignedPoints.ToString());
		}

		public void ToggleUpdating(bool toggle) => _isUpdating = toggle;
	}
}