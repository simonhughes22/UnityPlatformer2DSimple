using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit() {
        GameManager.Quit();
    }
}
