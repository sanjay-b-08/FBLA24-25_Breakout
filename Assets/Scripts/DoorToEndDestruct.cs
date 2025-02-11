using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;
//using static UnityEditor.Timeline.TimelinePlaybackControls;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class DoorToEndDestruct : MonoBehaviour
{
    public AudioSource doorOpen;

    private bool playerWin = false;

    private bool exitable;
    public bool exitableControl;
    
    private string currentSceneName;

    public Animator winAnim;

    public SkillsScript ss;
    public Timer timer;

    public Tilemap doorToEnd;

    public GameObject leaderboardScreen;

    public void Start()
    {
        leaderboardScreen.SetActive(false);
        playerWin = false;

        exitableControl = true;
    }

    public void Update()
    {
        if (exitableControl == true)
        {
            exitable = true;
        } else
        {
            exitable = false;
        }
        
        if (playerWin)
        {
            Time.timeScale = 0.05f;
            timer.timeIsRunning = false;
            //exitable = true;
            playerWins();

            if (exitable)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Time.timeScale = 1.0f;
                    currentSceneName = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene(currentSceneName);
                } 
                else if (Input.GetKeyDown(KeyCode.M))
                {
                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene(0);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    Application.Quit();
                }
                else if (Input.GetKeyDown(KeyCode.L))
                {
                    leaderboardScreen.SetActive(true);
                    exitableControl = false;
                }
            }
        }
    }
    public void endDestruct()
    {
        doorToEnd.GetComponent<TilemapRenderer>().enabled = false;
        doorToEnd.GetComponent<TilemapCollider2D>().enabled = false;
        doorToEnd.GetComponent<CompositeCollider2D>().enabled = false;
        doorToEnd.GetComponent<BoxCollider2D>().enabled = false;
        doorOpen.Play();

        playerWin = true;
        UnityEngine.Cursor.visible = true;

    }

    private void playerWins()
    {
        winAnim.SetBool("playerWin", true);
    }

}
