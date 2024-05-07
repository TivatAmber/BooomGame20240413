using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPanel : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}
