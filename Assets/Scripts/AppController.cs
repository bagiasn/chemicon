using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour {

    public int maxLifes;

    public AudioClip backgroundMusic;
    public AudioClip grabClip;

    private AudioSource audioSource;
    private int currentLifes;

    void Awake ()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("DDOL is on.");
    }

    void Start()
    {
        // Initialize global variables.
        currentLifes = maxLifes;

        audioSource = GetComponent<AudioSource>();

        // Start the MainMenu
        SceneManager.LoadScene(1);
    }

    public void PlayGrabClip()
    {
        audioSource.PlayOneShot(grabClip);
    }

    public void PlayBackgroundMusic()
    {
        
        audioSource.enabled = true;
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(backgroundMusic);

            
    }

    public void StopBackgroundMusic()
    {
        // Stop wont work, so disable it...
        audioSource.enabled = false;
    }

    public int GetRemainingLifes()
    {
        return currentLifes;
    }

    public void ReduceLifes()
    {
        Debug.Log("Lost one life");
        currentLifes--;
    }
}
