using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour {

    public int maxLifes;

    private int currentLifes;

    void Awake ()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Initialize life variable.
        currentLifes = maxLifes;

        // Start the MainMenu
        SceneManager.LoadScene(1);
    }

    public int GetRemainingLifes()
    {
        return currentLifes;
    }

    public void ReduceLifes()
    {
        currentLifes--;
    }
}
