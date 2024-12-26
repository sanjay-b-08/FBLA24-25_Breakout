using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;
using TMPro;

public class LeaderboardButton_StartMenu : MonoBehaviour
{
    public TextMeshProUGUI[] Entries;
    public string leaderboardKey;
    private int maxScores = 10;

    public GameObject startLeaderboardScreen;

    private void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                return;
            }
            //ShowScores();
        });
    }

    public void ShowCurScores()
    {
        startLeaderboardScreen.SetActive(true);

        LootLockerSDKManager.GetScoreList(leaderboardKey, maxScores, (response) =>
        {
            if (!response.success)
            {
                return;
            }

            LootLockerLeaderboardMember[] scores = response.items;

            for (int i = 0; i < scores.Length; i++)
            {
                Entries[i].text = (scores[i].rank + ". " + scores[i].member_id.ToUpper() + "                                                                                                                                            " + scores[i].score);
            }

            if (scores.Length < maxScores)
            {
                for (int i = scores.Length; i < maxScores; i++)
                {
                    Entries[i].text = (i + 1).ToString() + ". " + "XXX" + "                                                                                                                                            " + "None";
                }
            }

        });
    }

    public void exitLeaderboard()
    {
        startLeaderboardScreen.SetActive(false);
    }
}
