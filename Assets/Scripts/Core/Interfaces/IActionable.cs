using UnityEngine;

namespace Core.Interfaces
{
	public interface IActionable
	{
		bool CanExecute(GameObject target);
		bool CanExecute(Vector3 point);
		void Execute(GameObject target);
		void QueueExecution(GameObject target);
	}
}