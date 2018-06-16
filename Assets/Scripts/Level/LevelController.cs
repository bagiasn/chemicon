using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour {

    public Text rightText;
    public Text leftText;
    public Text restartText;
    public Text gameOverText;
    public Text collisionText;
    public Text repositoryText;
    public Text successText;
    public Text gatherHintText;

    private bool gameOver;
    private bool restart;
    private bool hasWon;

    private Dictionary<string, int> repo;

    void Start() {

        SetDefault();
    }

    void Update() {

        if (hasWon)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextLevel();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameOver();
                return;
            }
        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Restart();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameOver();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            AppController appInstance = FindObjectOfType<AppController>();
            appInstance.PlayGrabClip();

            CollectNearbyMols();
            // Check for completion only after a valid keystroke to avoid useless checks.
            checkForCompletion();
        }

    }

    public void UpdateColliderTag(String colTag)
    {
        collisionText.text = colTag;
    }

    public void SetTimerText(string newValue)
    {
        rightText.text = newValue;
        if (newValue == "Time's up!")
        {
            LevelTimedOut();
        }
    }

    public void SetGuideText(string newValue)
    {
        leftText.text = newValue;
    }

    private void GameOver()
    {
        Debug.Log("Game over");
        // Make it false to ensure that only one event will be handled.
        gameOver = false;

        AppController appInstance = FindObjectOfType<AppController>();
        appInstance.StopBackgroundMusic();

        SceneManager.LoadScene(1);
    }

    private void Restart()
    {
        Debug.Log("Restarting.");
        // Make it false to ensure that only one event will be handled.
        restart = false;
        // Stop sound for levels 1 & 3, since they have videos.
        if (SceneManager.GetActiveScene().buildIndex % 2 == 0)
        {
            AppController appInstance = FindObjectOfType<AppController>();
            appInstance.StopBackgroundMusic();
        }

        // Restart the scene.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void NextLevel()
    {
        Debug.Log("Loading next level.");
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        // Ignore space keystrokes on level 3.
        if (currentBuildIndex == 4) return;

        // Load the next one.
        int next = currentBuildIndex + 1;
        SceneManager.LoadScene(next);
    }

    private void CollectNearbyMols()
    {
        var playerObject = GameObject.Find("Player");
        Collider[] hitColliders = Physics.OverlapSphere(playerObject.transform.position, 2);
        foreach (Collider col in hitColliders)
        {
            string colTag = col.gameObject.tag;
            if (colTag == "Hydrogen" || colTag == "Oxygen" || colTag == "Calcium" || colTag == "Carbon"|| colTag == "Zinc" || colTag == "Chlorine")
            {
                UpdateRepository(col.gameObject);
            }
        }
    }

    private void SetDefault()
    {
        gameOver = false;
        restart = false;
        hasWon = false;

        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                leftText.text = "Leave the blue zone to start the game...";
                rightText.text = "...but first listen to what your teacher has to say!";
                break;
            case 3:
                leftText.text = "Collect the right molecules!";
                rightText.text = "";
                break;
            case 4:
                leftText.text = "Final level! You know what you have to do!";
                rightText.text = "";
                break;
            default:
                break;
        }
        restartText.text = "";
        gameOverText.text = "";
        successText.text = "";
        collisionText.text = "";
        repositoryText.text = "No molecules collected.";
        gatherHintText.text = "Press E to grab'em!";

        repo = new Dictionary<string, int>();
    }

    private void LevelTimedOut()
    {
        if (hasWon) return;

        AppController appInstance = FindObjectOfType<AppController>();
        if (appInstance.GetRemainingLifes() == 0)
        {
            Debug.Log("Game over");
            gameOverText.text = "Game Over";
            Invoke("GameOver", 3f);
        }
        else
        {
            appInstance.ReduceLifes();
            restartText.text = "You lost! To play again press space, to quit press Q";
            restart = true;
        }
    }

    private void UpdateRepository(GameObject molecule)
    {
        if (repo.ContainsKey(molecule.tag))
        {
            repo[molecule.tag]++;
        }
        else
        {
            repo.Add(molecule.tag, 1);
        }
        // First clean the previous text.
        repositoryText.text = "";
        foreach (KeyValuePair<string, int> entry in repo)
        {
            repositoryText.text += entry.Value + " " + entry.Key + " ";
        }
        Destroy(molecule);
    }

    private void checkForCompletion()
    {
        if (restart || gameOver) return;

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
                    TimerEvents timer = FindObjectOfType<TimerEvents>();
                    timer.Stop();
                }
                break;
            case "Level2":
                // The answer is 1 Zn / 2 Cl
                int rightItems2 = 0;
                foreach (KeyValuePair<string, int> entry in repo)
                {
                    if (entry.Key == "Zinc" && entry.Value == 1)
                    {
                        rightItems2++;
                    }
                    if (entry.Key == "Chlorine" && entry.Value == 2)
                    {
                        rightItems2++;
                    }
                }
                if (rightItems2 == 2)
                {
                    hasWon = true;
                    successText.text = "Great! Press space to play the next level or Q to quit";
                    TimerEvents timer = FindObjectOfType<TimerEvents>();
                    timer.Stop();
                }
                break;
            case "Level3":
                // The answer is 1 C / 10 O2
                int rightItems3 = 0;
                foreach (KeyValuePair<string, int> entry in repo)
                {
                    if (entry.Key == "Oxygen" && entry.Value == 10)
                    {
                        rightItems3++;
                    }
                    if (entry.Key == "Carbon" && entry.Value == 1)
                    {
                        rightItems3++;
                    }
                }
                if (rightItems3 == 2)
                {
                    hasWon = true;
                    successText.text = "You won! Press Q to quit";
                    TimerEvents timer = FindObjectOfType<TimerEvents>();
                    timer.Stop();
                }
                break;
            default:
                break;
        }
    }
}
