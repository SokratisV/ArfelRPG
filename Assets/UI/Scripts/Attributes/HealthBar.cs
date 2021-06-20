using RPG.Attributes;
using UnityEngine;

namespace RPG.UI
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] private Health health = null;
		[SerializeField] private RectTransform foreground = null;
		[SerializeField] private Canvas rootCanvas = null;

		private void OnEnable() => health.OnHealthChange += UpdateHealthBar;
		private void OnDisable() => health.OnHealthChange -= UpdateHealthBar;

		private void Start() => UpdateHealthBar(null, 0);

		private void UpdateHealthBar(GameObject _, float __)
		{
			if(Mathf.Approximately(health.GetFraction(), 0) || Mathf.Approximately(health.GetFraction(), 1))
			{
				rootCanvas.enabled = false;
				return;
			}

			rootCanvas.enabled = true;
			foreground.localScale = new Vector3(health.GetFraction(), 1, 1);
		}
	}
}