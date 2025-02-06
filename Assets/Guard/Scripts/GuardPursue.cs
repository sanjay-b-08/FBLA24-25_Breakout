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
    public LayerMask foreGround;

    public bool movable;
    public bool canCatch;

    private bool isGameOver;

    public Animator gameOverAnim;
    private string currentSceneName;

    public SkillsScript ss;

    public AudioSource guardShoutSound;
    private bool shoutSoundPlayed;

    public Timer timer;

    public bool isBribed;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        movable = true;
        canCatch = true;
        moveSpeed = 3f;

        shoutSoundPlayed = false;

        isBribed = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isGameOver)
        {
            Time.timeScale = 0f;
            ss.isGameOver = true;
            timer.timeIsRunning = false;
            GameOver();

            if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Try Again Pressed");
                currentSceneName = SceneManager.GetActiveScene().name;
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(currentSceneName);
            } else if (UnityEngine.Input.GetKeyDown(KeyCode.M))
            {
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(0);
            }

        }

        distance = Vector2.Distance(transform.position, player.transform.position);


        if (distance < 4f && movable)
        {
            /*if (!shoutSoundPlayed)
            {
                guardShoutSound.Play();
                shoutSoundPlayed= true;
            }*/

            Vector2 pos = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            if (noCollision(pos))
            {
                if (canTrackPlayer(player))
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

                    if (!shoutSoundPlayed)
                    {
                        guardShoutSound.Play();
                        shoutSoundPlayed = true;
                    }
                }
            }
        } else
        {
            shoutSoundPlayed = false;
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

    private bool canTrackPlayer(Transform player)
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.transform.position);

        //Debug.DrawRay(transform.position, direction * Mathf.Min(distance, maxRayDistance), Color.red);

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,  // Starting point
            direction,  // Direction
            Mathf.Min(distance, 4f),  // Maximum distance
            foreGround  // layer mask
        );

        if (hit.collider == null)
        {
            return true;
        }
        else
        {
            //Debug.Log("Obstacle in between");
            return false;
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
