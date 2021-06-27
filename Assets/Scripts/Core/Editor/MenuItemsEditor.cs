using UnityEditor;
using UnityEditor.SceneManagement;

public static class MenuItemsEditor
{
	[MenuItem("RPG/Scenes/Main Scene")]
	private static void OpenMainScene()
	{
		EditorSceneManager.SaveOpenScenes();
		EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
	}

	[MenuItem("RPG/Scenes/Test Scene")]
	private static void OpenTestScene()
	{
		EditorSceneManager.SaveOpenScenes();
		EditorSceneManager.OpenScene("Assets/Scenes/Test Scene.unity");
	}

	[MenuItem("RPG/Scenes/Sandbox Scene")]
	private static void OpenSandboxScene()
	{
		EditorSceneManager.SaveOpenScenes();
		EditorSceneManager.OpenScene("Assets/Scenes/Sandbox.unity");
	}
}