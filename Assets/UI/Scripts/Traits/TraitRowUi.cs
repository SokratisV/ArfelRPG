using System;
using RPG.Core;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class TraitRowUi : MonoBehaviour
	{
		[SerializeField] private Trait trait;
		[SerializeField] private TextMeshProUGUI valueText;
		[SerializeField] private Button minusButton, plusButton;

		private TraitStore _traitStore;

		private void Start()
		{
			_traitStore = PlayerFinder.Player.GetComponent<TraitStore>();
			minusButton.onClick.AddListener(() => Allocate(-1));
			plusButton.onClick.AddListener(() => Allocate(+1));
			_traitStore.OnStagedPointsChanged += UpdateUi;
			UpdateUi(Trait.Strength, 0);
		}

		private void OnDestroy() => _traitStore.OnStagedPointsChanged -= UpdateUi;

		private void UpdateUi(Trait _, int __)
		{
			plusButton.interactable = _traitStore.CanAssignPoints(trait, +1);
			minusButton.interactable = _traitStore.CanAssignPoints(trait, -1);
			valueText.SetText(_traitStore.GetProposedPoints(trait).ToString());
		}

		private void Allocate(int points) => _traitStore.AssignPoints(trait, points);
	}
}