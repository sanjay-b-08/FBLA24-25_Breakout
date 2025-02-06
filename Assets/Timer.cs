using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeRemaining = 0;
    public bool timeIsRunning = true;
    public TextMeshProUGUI timeText;

    public float minutes;
    public float seconds;

    private int timeScore = 1500;

    private int finalScore;

    public LeaderboardManager lm;
    public TextMeshProUGUI score;

    public TextMeshProUGUI updatingScoreText;

    void Start()
    {
        timeIsRunning = true;

        //Starts scoring based on time
        InvokeRepeating("decreaseScore", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeIsRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
                updatingScoreText.SetText("Overall Score: " + timeScore);
            }
        } else
        {
            //Ends scoring based on time
            CancelInvoke("decreaseScore");
            finalScore = timeScore;

            //Code that affects leaderboard manager
            lm.playerScore = finalScore;
            score.SetText("Score: " + lm.playerScore);
            //lm.ShowScores();
        }

        //Stop score decrease if player takes longer than 10 minutes
        if (minutes >= 10)
        {
            CancelInvoke("decreaseScore");
        }

        //Prevents score from being negative
        if (timeScore < 0)
        {
            timeScore = 0;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        minutes = Mathf.FloorToInt(timeToDisplay / 60);
        seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

    private void decreaseScore()
    {
        timeScore -= 2;
        //Debug.Log(timeScore);
        //yield return new WaitForSeconds(1);
    }

    public int getTimeScore()
    {
        return timeScore;
    }

    public void setTimeScore(int decrement)
    {
        timeScore -= decrement;
    }
    public int getFinalScore()
    {
        return finalScore;
    }

    /*public void setTimeScore(int decrement)
    {
        timeScore -= decrement;
    }*/
}
