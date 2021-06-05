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

		private void Start()
		{
			_traitStore = PlayerFinder.Player.GetComponent<TraitStore>();
			commitButton.onClick.AddListener(_traitStore.Commit);
			_traitStore.OnStagedPointsChanged += UpdateUi;
			UpdateUi(Trait.Constitution, 0);
		}

		private void UpdateUi(Trait _, int __) => unassignedPoints.SetText(_traitStore.UnassignedPoints.ToString());
	}
}