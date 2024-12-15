using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GuardPursue : MonoBehaviour
{
    public static float moveSpeed;
    public Transform player;
    public float catchRange = 0.5f;

    private float distance;

    public LayerMask playerMask;
    public LayerMask solids;

    public bool movable;
    public bool canCatch;

    private bool isGameOver;

    public Animator gameOverAnim;
    private string currentSceneName;

    public SkillsScript ss;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        movable = true;
        canCatch = true;
        moveSpeed = 3f;
    }

    // Update is called once per frame
    void Update()
    {

        if (isGameOver)
        {
            Time.timeScale = 0.05f;
            ss.isGameOver = true;
            GameOver();

            if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Try Again Pressed");
                currentSceneName = SceneManager.GetActiveScene().name;
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(currentSceneName);
            }

        }

        distance = Vector2.Distance(transform.position, player.transform.position);


        if (distance < 4f && movable)
        {
            Vector2 pos = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            if (noCollision(pos))
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            }
        }

        if (caught(transform.position) && canCatch)
        {
            isGameOver = true;
        }
    }

    private bool noCollision(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.3f, solids) != null)
        {
            return false;
        }
        return true;
    }


    private void GameOver()
    {
        gameOverAnim.SetBool("isGameOver", true);
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
    }

    public bool getIsGameOver()
    {
        return isGameOver;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 4);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRange);
    }
}
