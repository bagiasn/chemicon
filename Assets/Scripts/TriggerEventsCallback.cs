using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TriggerEventsCallback : MonoBehaviour
{
    public int timeLeft;
    public Text rightText;
    public Text leftText;

    private bool gameStarted;

    // Use this for initialization
    void Start()
    {
        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            rightText.text = ("Time Left: " + timeLeft);
            if (timeLeft <= 0)
            {
                StopCoroutine(Timer());
                rightText.text = "Time's up!";
                GameController controller = FindObjectOfType<GameController>();
                if (!controller.hasLevelEnded())
                {
                    controller.LevelTimedOut();
                }
            }
        }
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player exited the blue zone.");
            gameStarted = true;
            leftText.text = "Find your exercise and collect the right molecules";
            VideoPlayer videoPlayer = FindObjectOfType<VideoPlayer>();
            videoPlayer.Stop();
            AudioSource audio = videoPlayer.GetComponentInParent<AudioSource>();
            audio.Stop();

            GameController controller = FindObjectOfType<GameController>();
            controller.EnableMusic();

            StartCoroutine(Timer());
        }
    }

    public void StopTimer()
    {
        StopCoroutine(Timer());

        gameStarted = false;
    }
}
