using UnityEngine;
using UnityEngine.AI;

namespace RPG.Skills.Behaviors
{
	public class Blink : SkillBehavior
	{
		public override void BehaviorStart(GameObject user, GameObject[] targets, Vector3? point = null)
		{
			if(!point.HasValue) return;
			user.GetComponent<NavMeshAgent>().Warp(point.Value);
			base.BehaviorStart(user, targets, point);
		}
	}
}