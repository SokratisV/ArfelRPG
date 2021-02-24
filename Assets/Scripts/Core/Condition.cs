using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;

namespace RPG.Core
{
	[System.Serializable]
	public class Condition
	{
		[SerializeField] private string predicate;
		[SerializeField] private string[] parameters;

		public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
		{
			foreach(var evaluator in evaluators)
			{
				var result = evaluator.Evaluate(predicate, parameters);
				if(result == null) continue;
				if(result == false) return false;
			}

			return true;
		}
	}
}