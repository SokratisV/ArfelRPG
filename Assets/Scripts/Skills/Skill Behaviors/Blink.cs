using System.Collections;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Skills.Behaviors
{
	public class Blink : SkillBehavior
	{
		[SerializeField] [Range(0, 20f)] private float distance;

		public override bool HasCastTime() => false;

		public override bool UseExtraAnimation() => true;

		public override int SkillAnimationNumber() => 3;

		public override float GetCastingRange() => distance;

		public override void BehaviorStart(SkillData data)
		{
			if(!data.Point.HasValue) return;
			var path = new NavMeshPath();
			if(NavMesh.CalculatePath(data.Targets[0].transform.position, data.Point.Value, NavMesh.AllAreas, path))
			{
				data.Targets[0].GetComponent<Health>().IsInvulnerable = true;
				if(Helper.IsWithinDistance(data.Point.Value, data.Targets[0].transform.position, distance))
				{
					data.Targets[0].GetComponent<Mover>().Blink(data.Point.Value);
					base.BehaviorStart(data);
					return;
				}

				var finalPoint = Helper.CalculateMaximumDistanceNavMeshPoint(path, distance);
				if(finalPoint == default)
				{
					finalPoint = data.Point.Value;
				}

				data.Targets[0].GetComponent<Mover>().Blink(finalPoint);
			}

			else return;

			base.BehaviorStart(data);
		}

		public override IEnumerator BehaviorUpdate(SkillData data)
		{
			yield break;
		}

		public override void BehaviorEnd(SkillData data)
		{
			data.Targets[0].GetComponent<Health>().IsInvulnerable = false;
			base.BehaviorEnd(data);
		}
	}
}