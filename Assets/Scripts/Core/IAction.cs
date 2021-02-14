using System;

namespace RPG.Core
{
    public interface IAction
    {
        event Action OnComplete;
        void Cancel();
        void Complete();
        void ExecuteAction(IActionData data);
    }
}