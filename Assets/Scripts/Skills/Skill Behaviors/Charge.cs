﻿using System.Collections;
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

		public override void BehaviorStart(SkillData data)
		{
			if(!data.Point.HasValue) return;
			var path = new NavMeshPath();
			if(NavMesh.CalculatePath(data.User.transform.position, data.Point.Value, NavMesh.AllAreas, path))
			{
				var finalPoint = Helper.CalculateMaximumDistanceNavMeshPoint(path, distance);
				if(finalPoint == default)
				{
					finalPoint = data.Point.Value;
				}

				data.User.GetComponent<Mover>().Dash(finalPoint, dashDuration);
				data.User.GetComponent<Health>().IsInvulnerable = true;
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
			data.User.GetComponent<Health>().IsInvulnerable = false;
			base.BehaviorEnd(data);
		}
	}
}