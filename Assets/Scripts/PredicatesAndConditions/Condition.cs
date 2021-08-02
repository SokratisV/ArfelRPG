using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.PAC
{
	[System.Serializable]
	public class Condition
	{
		[SerializeField] private Disjunction[] and;

		public bool Check(IEnumerable<IPredicateEvaluator> evaluators) => and.All(disjunction => disjunction.Check(evaluators));

		[System.Serializable]
		class Disjunction
		{
			[SerializeField] private Predicate[] or;

			public bool Check(IEnumerable<IPredicateEvaluator> evaluators) => or.Any(predicate => predicate.Check(evaluators));
		}

		[System.Serializable]
		public class Predicate
		{
			[SerializeField] private RPG.PAC.Predicate predicate;
			[SerializeField] private string[] parameters;
			[SerializeField] private bool negate;
			
			public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
			{
				foreach(var evaluator in evaluators)
				{
					var result = evaluator.Evaluate(predicate, parameters);
					if(result == negate) return false;
				}

				return true;
			}
		}
	}
}