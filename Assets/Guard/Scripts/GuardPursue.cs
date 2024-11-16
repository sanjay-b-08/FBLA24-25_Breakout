using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GuardPursue : MonoBehaviour
{
    public float moveSpeed;
    public Transform player;
    public float catchRange = 1f;


    private float distance;

    public GameObject Player;
    private int playerKillCount;

    public LayerMask playerMask;

    public Image caughtScreen;
    public TextMeshProUGUI caughtText;
    public Button tryAgainButton;
    public TextMeshProUGUI tryAgainText;

    // Start is called before the first frame update
    void Start()
    {
        playerKillCount = Player.GetComponent<SkillsScript>().killCount;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        moveSpeed = moveSpeed + (0.3f * playerKillCount);

        if (distance < 4)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }

        if (caught(transform.position))
        {
            //Debug.Log("Player Caught!");
            Time.timeScale = 0.2f;
            GameOver();
        }
    }

    private bool caught(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, catchRange, playerMask) != null)
        {
            return true;
        }
        return false;
    }

    private void GameOver()
    {
        //Red screen comes into view
        if (caughtScreen.color.a < 1)
        {
            var colorAlpha = caughtScreen.color;
            colorAlpha.a += 0.01f;
            caughtScreen.color = colorAlpha;
        }

        if (caughtText.color.a < 1)
        {
            var colorAlpha = caughtText.color;
            colorAlpha.a += 0.005f;
            caughtText.color = colorAlpha;
        }

        tryAgainButton.GetComponent<Button>().enabled = true;

        if (tryAgainButton.GetComponent<Image>().color.a < 1)
        {
            var colorAlpha = tryAgainButton.GetComponent<Image>().color;
            colorAlpha.a += 0.005f;
            tryAgainButton.GetComponent<Image>().color = colorAlpha;
        }

        if (tryAgainText.color.a < 1)
        {
            var colorAlpha = tryAgainText.color;
            colorAlpha.a += 0.005f;
            tryAgainText.color = colorAlpha;
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 4);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRange);
    }
}
