using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class DoorToEndDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap doorToEnd;

    public Image congratsScreen;
    public TextMeshProUGUI congratsText;
    public Button playAgainButton;
    public TextMeshProUGUI playAgainText;

    public void endDestruct()
    {
        doorToEnd.GetComponent<TilemapRenderer>().enabled = false;
        doorToEnd.GetComponent<TilemapCollider2D>().enabled = false;
        doorToEnd.GetComponent<CompositeCollider2D>().enabled = false;
        doorToEnd.GetComponent<BoxCollider2D>().enabled = false;

        GameOver();

    }
    void GameOver()
    {
        if (congratsScreen.color.a < 1)
        {
            var colorAlpha = congratsScreen.color;
            colorAlpha.a += 0.01f;
            congratsScreen.color = colorAlpha;
        }

        if (congratsText.color.a < 1)
        {
            var colorAlpha = congratsText.color;
            colorAlpha.a += 0.005f;
            congratsText.color = colorAlpha;
        }

        playAgainButton.GetComponent<Button>().enabled = true;

        if (playAgainButton.GetComponent<Image>().color.a < 1)
        {
            var colorAlpha = playAgainButton.GetComponent<Image>().color;
            colorAlpha.a += 0.005f;
            playAgainButton.GetComponent<Image>().color = colorAlpha;
        }

        if (playAgainText.color.a < 1)
        {
            var colorAlpha = playAgainText.color;
            colorAlpha.a += 0.005f;
            playAgainText.color = colorAlpha;
        }
    }

}
