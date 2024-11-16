using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorToEndDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap doorToEnd;

    public void endDestruct()
    {
        doorToEnd.GetComponent<TilemapRenderer>().enabled = false;
        doorToEnd.GetComponent<TilemapCollider2D>().enabled = false;
        doorToEnd.GetComponent<CompositeCollider2D>().enabled = false;
        doorToEnd.GetComponent<BoxCollider2D>().enabled = false;
    }
}
