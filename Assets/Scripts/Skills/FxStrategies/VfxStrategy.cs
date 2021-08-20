using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Skills
{
	[CreateAssetMenu(menuName = "RPG/Skills/Vfx Strategy", fileName = "VfxStrategy")]
	public class VfxStrategy : EffectsStrategy
	{
		[SerializeField, ValueDropdown(nameof(_positionChoice))]
		private int targetChoice;

		[SerializeField] private GameObject[] vfx;
		[SerializeField, Range(0, 15)] private float destroyAfter;

		public override void ExecuteStrategy(SkillData data)
		{
			switch (targetChoice)
			{
				case 0:
				{
					SpawnVfx(data.User.transform.position, vfx, destroyAfter);
					break;
				}
				case 1:
				{
					SpawnVfx(data.InitialTarget.transform.position, vfx, destroyAfter);
					break;
				}
				case 2:
				{
					SpawnVfx(data.Point, vfx, destroyAfter);
					break;
				}
				case 3:
				{
					SpawnVfx(data.Targets, vfx, destroyAfter);
					break;
				}
			}
		}

		private static void SpawnVfx(IEnumerable<GameObject> targets, GameObject[] vfx, float destroyAfter)
		{
			foreach (var target in targets)
			{
				foreach (var gameObject in vfx)
				{
					Destroy(Instantiate(gameObject, target.transform.position, Quaternion.identity), destroyAfter);
				}
			}
		}

		private static void SpawnVfx(Vector3? position, GameObject[] vfx, float destroyAfter)
		{
			if (!position.HasValue) return;
			foreach (var gameObject in vfx)
			{
				Destroy(Instantiate(gameObject, position.Value, Quaternion.identity), destroyAfter);
			}
		}

		private static IEnumerable _positionChoice = new ValueDropdownList<int>
		{
			{"User", 0},
			{"Target", 1},
			{"Point", 2},
			{"Targets", 3}
		};
	}
}