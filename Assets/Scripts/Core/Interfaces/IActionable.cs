using UnityEngine;

namespace Core.Interfaces
{
	public interface IActionable
	{
		bool CanExecute(GameObject target);
		void Execute(GameObject target);
		void QueueExecution(GameObject target);
	}
}