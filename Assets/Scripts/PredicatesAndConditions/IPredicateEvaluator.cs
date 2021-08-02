namespace RPG.PAC
{
	public interface IPredicateEvaluator
	{
		public bool? Evaluate(Predicate predicate, string[] parameters);
	}
}