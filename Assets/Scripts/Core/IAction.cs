namespace RPG.Core
{
    public interface IAction
    {
        void Cancel();
        void Complete();
        void ExecuteAction(ActionData data);
    }
}