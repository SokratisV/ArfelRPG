using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

public class HealthOrbDisplay : MonoBehaviour
{
	private Health _playerHealth;
	[SerializeField] private Image image;

	private void Start()
	{
		_playerHealth = PlayerFinder.Player.GetComponent<Health>();
		_playerHealth.OnTakeDamage += CalculateHealthFill;
	}

	private void CalculateHealthFill(GameObject _, float __) => image.fillAmount = _playerHealth.GetFraction();
}