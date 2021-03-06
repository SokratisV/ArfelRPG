using RPG.Core;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Skills.Behaviors
{
	public class Blink : SkillBehavior
	{
		[SerializeField] [Range(0, 20f)] private float distance;

		public override void BehaviorStart(GameObject user, GameObject[] targets, Vector3? point = null)
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

				user.GetComponent<Mover>().Blink(finalPoint);
			}

			else return;

			base.BehaviorStart(user, targets, point);
		}
	}
}