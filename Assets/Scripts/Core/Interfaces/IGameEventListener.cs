namespace Core.Interfaces
{
	public interface IGameEventListener<in T>
	{
		public void RaiseEvent(T data);
	}
}