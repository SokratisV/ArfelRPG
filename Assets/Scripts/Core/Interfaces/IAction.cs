using System;

namespace RPG.Core
{
	public interface IAction
	{
		event Action OnActionComplete;
		void CancelAction();
		void CompleteAction();
		void ExecuteQueuedAction(IActionData data);
	}
}