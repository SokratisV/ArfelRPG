using UnityEngine;

public class PowerUp : MonoBehaviour
{
	public GameObject pickupEffect;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Pickup();
		}
	}

	private void Pickup()
	{
		Instantiate(pickupEffect, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}