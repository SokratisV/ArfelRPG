using System.Collections.Generic;
using UnityEngine;

namespace RPG.Skills.Behaviors
{
	public abstract class TargetBehavior : ScriptableObject
	{
		//-1 is unlimited range
		[Range(-1, 20)] [SerializeField] protected float minimumClickRange;
		public virtual float GetRadius() => 0;
		
		public abstract TargetType TargetType();
		public virtual float GetMinRange() => minimumClickRange;

		//Return false when it only cares about the point
		public abstract List<GameObject> GetTargets(GameObject user, GameObject initialTarget = null, Vector3? raycastPoint = null, Vector3? direction = null);
	}
}