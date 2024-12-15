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
    private bool playerWin = false;
    private string currentSceneName;

    public Animator winAnim;

    public SkillsScript ss;

    public Tilemap doorToEnd;

    public void Start()
    {
        playerWin = false;
    }

    public void Update()
    {
        if (playerWin)
        {
            Time.timeScale = 0.05f;
            ss.isGameOver = true;
            playerWins();

            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Replay Pressed");
                Time.timeScale = 1.0f;
                currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentSceneName);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Quitting game...");
                Application.Quit();
            }
        }
    }
    public void endDestruct()
    {
        doorToEnd.GetComponent<TilemapRenderer>().enabled = false;
        doorToEnd.GetComponent<TilemapCollider2D>().enabled = false;
        doorToEnd.GetComponent<CompositeCollider2D>().enabled = false;
        doorToEnd.GetComponent<BoxCollider2D>().enabled = false;

        playerWin = true;

    }

    private void playerWins()
    {
        winAnim.SetBool("playerWin", true);
    }

}
