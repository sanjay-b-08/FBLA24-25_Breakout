using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public Timer timer;
    [HideInInspector] public int playerScore;

    public TMP_InputField playerID;
    //public TextMeshProUGUI score;

    public string leaderboardKey;

    private int maxScores = 10;
    public TextMeshProUGUI[] Entries;

    public GameObject leaderboardScreen;
    public Button submitButton;
    public TextMeshProUGUI submitScoreText;

    public DoorToEndDestruct dte;

    private void Start()
    {
        submitButton.interactable = true;
        submitScoreText.SetText("Submit Score");

        playerID.characterLimit = 3;

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                return;
            }
            ShowScores();
        });
    }

    public void SubmitAndShowScore()
    {
        LootLockerSDKManager.SubmitScore(playerID.text, playerScore, leaderboardKey, (response) =>
        {
            if (!response.success)
            {
                return;
            }
            submitButton.interactable = false;
            submitScoreText.SetText("Submitted");
            ShowScores();

        });
    }

    public void ShowScores()
    {
        LootLockerSDKManager.GetScoreList(leaderboardKey, maxScores, (response) =>
        {
            if (!response.success)
            {
                return;
            }

            LootLockerLeaderboardMember[] scores = response.items;

            for (int i = 0; i < scores.Length; i++)
            {
                Entries[i].text = (scores[i].rank + ".   " + scores[i].member_id.ToUpper() + "             " + scores[i].score);
            }

            if (scores.Length < maxScores)
            {
                for (int i = scores.Length; i < maxScores; i++)
                {
                    Entries[i].text = (i + 1).ToString() + ".   " + "XXX" + "             " + "None";
                }
            }

        });
    }

    public void exitLeaderboard()
    {
        dte.exitableControl = true;
        leaderboardScreen.SetActive(false);
    }
}
