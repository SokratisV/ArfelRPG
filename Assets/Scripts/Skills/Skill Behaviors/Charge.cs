using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Skills.Behaviors
{
	public class Charge : SkillBehavior
	{
		[SerializeField] [Range(0, 20f)] private float distance;
		[SerializeField] [Range(0, 2)] private float dashDuration;
		
		public override bool HasCastTime() => false;
		public override bool UseExtraAnimation() => true;
		public override int SkillAnimationNumber() => 3;
		
		public override float GetCastingRange() => distance;

		public override void BehaviorStart(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			if(!point.HasValue) return;
			var path = new NavMeshPath();
			if(NavMesh.CalculatePath(user.transform.position, point.Value, NavMesh.AllAreas, path))
			{
				var finalPoint = Helper.CalculateMaximumDistanceNavMeshPoint(path, distance);
				if(finalPoint == default)
				{
					finalPoint = point.Value;
				}

				user.GetComponent<Mover>().Dash(finalPoint, dashDuration);
				user.GetComponent<Health>().IsInvulnerable = true;
			}
			else return;

			base.BehaviorStart(user, targets, point);
		}

		public override void BehaviorUpdate(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			
		}

		public override void BehaviorEnd(GameObject user, List<GameObject> targets, Vector3? point = null)
		{
			user.GetComponent<Health>().IsInvulnerable = false;
			base.BehaviorEnd(user, targets, point);
		}
	}
}