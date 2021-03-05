using UnityEngine;

namespace Core.Interfaces
{
	public interface ICombatActionable
	{
		bool CanExecute(GameObject target);
		void Execute(GameObject target);
		void QueueExecution(GameObject target);
	}
}