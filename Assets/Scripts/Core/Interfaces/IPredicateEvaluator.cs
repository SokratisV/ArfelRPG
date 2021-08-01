namespace RPG.Core.Interfaces
{
	public interface IPredicateEvaluator
	{
		bool? Evaluate(Predicate predicate, string[] parameters);
	}
}