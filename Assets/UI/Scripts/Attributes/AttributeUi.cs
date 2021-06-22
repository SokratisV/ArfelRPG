using RPG.Core;
using RPG.Stats;
using TMPro;
using UnityEngine;

namespace UI.Scripts.Attributes
{
	public class AttributeUi : MonoBehaviour
	{
		[SerializeField] private Trait traitToShow;
		[SerializeField] private TextMeshProUGUI attributeText, attributeValue;
		[SerializeField] private bool excludeFromUpdating;
		
		public Trait Trait => traitToShow;

		private TraitStore _baseStats;
		private static bool _isUpdating;

		private void Awake() => _baseStats = PlayerFinder.Player.GetComponent<TraitStore>();

		private void Update()
		{
			if (_isUpdating && !excludeFromUpdating)
			{
				attributeValue.SetText(_baseStats.GetPoints(traitToShow).ToString());
			}
		}

		private void Start() => attributeText.SetText(traitToShow.ToString());

		public void ToggleUpdating(bool toggle) => _isUpdating = toggle;
	}
}