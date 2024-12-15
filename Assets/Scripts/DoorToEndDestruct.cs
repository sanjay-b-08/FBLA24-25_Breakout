using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class DoorToEndDestruct : MonoBehaviour
{
    private bool playerWin = false;

    public Tilemap doorToEnd;

    public void Start()
    {
        //replayButton.GetComponent<Button>().enabled = false;
    }
    public void endDestruct()
    {
        doorToEnd.GetComponent<TilemapRenderer>().enabled = false;
        doorToEnd.GetComponent<TilemapCollider2D>().enabled = false;
        doorToEnd.GetComponent<CompositeCollider2D>().enabled = false;
        doorToEnd.GetComponent<BoxCollider2D>().enabled = false;

        playerWin = true;

        if (playerWin)
        {
            playerWins();
            Time.timeScale = 0.05f;
        }

    }

    private void playerWins()
    {
        
    }

}
