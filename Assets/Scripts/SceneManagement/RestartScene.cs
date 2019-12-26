using System.Collections;
using RPG.Core;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    public void Restart()
    {
        StartCoroutine(LoadScene());
    }

    public void ShowDeathUI()
    {
        canvas.SetActive(true);
    }

    private IEnumerator LoadScene()
    {
        FindObjectOfType<SavingWrapper>().Delete();
        FindObjectOfType<MusicManager>().ResetMusicPlayer();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        canvas.SetActive(false);
    }
}
