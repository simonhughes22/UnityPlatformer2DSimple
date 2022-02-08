using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
