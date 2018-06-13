using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public int maxLifes;
    public int moleculesEachWave;
    
    public Text restartText;
    public Text gameOverText;
    public Text collisionText;

    public GameObject molecule;
    public Vector3 spawnValues;

    public AudioClip backgroundMusic;
    public AudioClip grabClip;
    private AudioSource audioSource;

    private bool gameOver;
    private bool restart;
    private bool hasWon;

    private int currentLifes;

    private GameObject collidingGameObject;

    void Start () {

        DontDestroyOnLoad(gameObject);

        gameOver = false;
        restart = false;
        hasWon = false;
        restartText.text = "";
        gameOverText.text = "";
        currentLifes = maxLifes;

        audioSource = GetComponent<AudioSource>();
    }
	
	void Update () {

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
                collidingGameObject.SetActive(false);
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

    private void GameOver()
    {
        gameOver = true;
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("MainMenu");
    }

    public bool hasLevelEnded()
    {
        return gameOver || restart;
    }

}
