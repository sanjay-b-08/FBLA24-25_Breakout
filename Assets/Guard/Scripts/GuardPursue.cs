using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GuardPursue : MonoBehaviour
{
    public float moveSpeed;
    public Transform player;
    public float catchRange = 1f;

    private float distance;

    public LayerMask playerMask;

    public Image caughtScreen;
    public TextMeshProUGUI caughtText;
    public Button tryAgainButton;
    public TextMeshProUGUI tryAgainText;

    public bool movable;

    private bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        movable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            Time.timeScale = 0.05f;
            GameOver();
        }

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 4 && movable)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }

        if (caught(transform.position))
        {
            isGameOver = true;
        }
    }

    private void GameOver()
    {
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

    private bool caught(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, catchRange, playerMask) != null)
        {
            //Debug.Log("Player Caught!");
            return true;
        }
        return false;
    }

    public void increaseGuardSpeed(float add)
    {
        moveSpeed += add;
        Debug.Log("Speed increased");
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 4);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRange);
    }
}
