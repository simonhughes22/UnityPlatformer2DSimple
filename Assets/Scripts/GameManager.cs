using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    public int bottomOfScreenYVal = -200;

    private AudioSource audioSource;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip) {
        Instance.audioSource.PlayOneShot(clip);
    }

    public static void Quit() {        
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MainScene");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }
}
