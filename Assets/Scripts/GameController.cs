using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int maxLifes;
    public int moleculesEachWave;
    
    public Text restartText;
    public Text gameOverText;
    public Text collisionText;
    public Text repositoryText;
    public Text successText;
    public Text gatherHintText;

    public GameObject molecule;
    public Vector3 spawnValues;

    public AudioClip backgroundMusic;
    public AudioClip grabClip;
    private AudioSource audioSource;

    private bool gameOver;
    private bool restart;
    private bool hasWon;

    private int currentLifes;
    private Dictionary<string, int> repo;

    private GameObject collidingGameObject;

    void Start () {

        DontDestroyOnLoad(gameObject);

        gameOver = false;
        restart = false;
        hasWon = false;
        restartText.text = "";
        gameOverText.text = "";
        collisionText.text = "";
        repositoryText.text = "No molecules collected.";
        gatherHintText.text = "Press E to grab'em!";

        currentLifes = maxLifes;
        repo = new Dictionary<string, int>();
        audioSource = GetComponent<AudioSource>();
    }
	
	void Update () {

        if (hasWon)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<AudioSource>().Stop();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameOver();
            }
        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<AudioSource>().Stop();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameOver();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (collidingGameObject != null)
            {
                audioSource.PlayOneShot(grabClip);
                // Remove the molecule.
                collidingGameObject.SetActive(false);
                // Store its value in our repository.
                UpdateRepository();
                // Check for completion only after a valid keystroke to avoid useless checks.
                checkForCompletion();
                // This could happen onTriggerEnter so there may be no onTriggerExit.
                // So clean up the collisionText here.
                collisionText.text = "";
            }
        }

    }

    public void EnableMusic()
    {
        audioSource.PlayOneShot(backgroundMusic);
    }

    public void LevelTimedOut()
    {
        if (currentLifes == 0)
        {
            gameOverText.text = "Game Over";
            Invoke("GameOver", 2f);
        }
        else
        {
            currentLifes--;
            restartText.text = "You lost! To play again press space, to quit press Q";
            restart = true;
        }
    }

    public void updateColliderTag(String tag, GameObject gameObject)
    {
        if (tag == "")
        {
            collidingGameObject = null;
        }
        else
        {
            collidingGameObject = gameObject;
        }
        collisionText.text = tag;
    }

    public bool hasLevelEnded()
    {
        return gameOver || restart;
    }

    private void GameOver()
    {
        gameOver = true;
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateRepository()
    {
        if (repo.ContainsKey(collidingGameObject.tag))
        {
            repo[collidingGameObject.tag]++;
        }
        else
        {
            repo.Add(collidingGameObject.tag, 1);
        }
        // First clean the previous text.
        repositoryText.text = "";
        foreach (KeyValuePair<string, int> entry in repo)
        {
            repositoryText.text += entry.Value + " " + entry.Key + " ";
        }
        collidingGameObject = null;
    }

    private void checkForCompletion()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "Level1":
                // The answer is 1 Ca / 1 C
                int rightItems = 0;
                foreach (KeyValuePair<string, int> entry in repo)
                {
                    if (entry.Key == "Carbon" && entry.Value == 1)
                    {
                        rightItems++;
                    }
                    if (entry.Key == "Calcium" && entry.Value == 1)
                    {
                        rightItems++;
                    }
                }
                if (rightItems == 2)
                {
                    hasWon = true;
                    successText.text = "That's right! Press space to play the next level or Q to quit";
                    TriggerEventsCallback timer = FindObjectOfType<TriggerEventsCallback>();
                    timer.StopTimer();
                }
                break;
            default:
                break;
        }
    }
}
