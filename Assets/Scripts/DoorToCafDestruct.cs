using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorToCafDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap doorToCaf;

    public void cafDestruct()
    {
        doorToCaf.GetComponent<TilemapRenderer>().enabled = false;
        doorToCaf.GetComponent<TilemapCollider2D>().enabled = false;
        doorToCaf.GetComponent<CompositeCollider2D>().enabled = false;
        doorToCaf.GetComponent<BoxCollider2D>().enabled = false;
    }
}
