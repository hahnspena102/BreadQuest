using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene(4);
    }
    public void QuitGame() {
        Application.Quit();
    }
}
