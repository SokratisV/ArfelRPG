using UnityEngine;

public class TimeController : MonoBehaviour
{
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			Time.timeScale += .1f;
		}

		if(Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			Time.timeScale -= .1f;
		}
	}
}