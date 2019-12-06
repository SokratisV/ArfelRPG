using System.Collections;
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        canvas.SetActive(false);
    }
}
