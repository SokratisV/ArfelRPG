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

		[SerializeField] private bool parent;
		[SerializeField] private Vector3 offset;
		[SerializeField] private GameObject[] vfx;
		[SerializeField, Range(0, 15)] private float destroyAfter;

		public override void ExecuteStrategy(SkillData data)
		{
			switch (targetChoice)
			{
				case 0:
				{
					SpawnVfx(data.User);
					break;
				}
				case 1:
				{
					SpawnVfx(data.InitialTarget);
					break;
				}
				case 2:
				{
					SpawnVfx(data.Point);
					break;
				}
				case 3:
				{
					SpawnVfx(data.Targets);
					break;
				}
			}
		}

		private void SpawnVfx(GameObject target)
		{
			foreach (var fx in vfx)
			{
				var instance = Instantiate(fx);
				instance.transform.position = target.transform.position + offset;
				if (parent) instance.transform.SetParent(target.transform);
				Destroy(instance, destroyAfter);
			}
		}

		private void SpawnVfx(IEnumerable<GameObject> targets)
		{
			foreach (var target in targets)
			{
				SpawnVfx(target);
			}
		}

		private void SpawnVfx(Vector3? position)
		{
			if (!position.HasValue) return;
			foreach (var fx in vfx)
			{
				var instance = Instantiate(fx);
				instance.transform.position = position.Value + offset;
				Destroy(instance, destroyAfter);
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