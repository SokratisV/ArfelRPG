namespace RPG.Core.Interfaces
{
	public interface IPredicateEvaluator
	{
		bool? Evaluate(DialoguePredicates predicate, string[] parameters);
	}
}