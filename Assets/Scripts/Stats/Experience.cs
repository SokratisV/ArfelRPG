using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats
{
	public class Experience : MonoBehaviour, ISaveable
	{
		[SerializeField] private float experiencePoints = 0;

		public event Action<float> OnExperienceGained;

		public void GainExperience(float experience)
		{
			experiencePoints += experience;
			OnExperienceGained?.Invoke(experience);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.K))
			{
				AddTestExperience();
			}
		}

		[ContextMenu("Add 10 experience points")]
		public void AddTestExperience() => GainExperience(10);

		public object CaptureState() => experiencePoints;

		public void RestoreState(object state) => experiencePoints = (float) state;

		public float GetPoints() => experiencePoints;
	}
}