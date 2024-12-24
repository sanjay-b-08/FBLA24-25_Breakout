using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreInfoScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject scoreInfo;

    public SkillsScript ss;
    public Timer timer;
    public LeaderboardManager lm;

    public TextMeshProUGUI timeScoreInfo;
    public TextMeshProUGUI killScoreInfo;
    public TextMeshProUGUI totalScoreInfo;

    private void Start()
    {
        scoreInfo.SetActive(false);
    }
    public void showInfoScreen()
    {
        scoreInfo.SetActive(true);
        timeScoreInfo.SetText("Score based on time: " + (timer.getFinalScore() + (ss.killCount * 50)));
        killScoreInfo.SetText("Score reduction based on kill count: " + (ss.killCount * 50));
        totalScoreInfo.SetText("Total score: " + lm.playerScore);
    }

    public void goBackToLeaderboardScreen()
    {
        scoreInfo.SetActive(false);
    }
}
