namespace RPG.Core
{
	public enum GameState
	{
		MainMenu,
		InGame
	}

	public static class GlobalGameState
	{
		public static GameState GameState { get; set; } = GameState.MainMenu;
	}
}