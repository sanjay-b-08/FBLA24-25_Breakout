using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorToRecDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap doorToRec;
    public AudioSource doorOpen;

    public void recDestruct()
    {
        doorToRec.GetComponent<TilemapRenderer>().enabled = false;
        doorToRec.GetComponent<TilemapCollider2D>().enabled = false;
        doorToRec.GetComponent<CompositeCollider2D>().enabled = false;
        doorToRec.GetComponent<BoxCollider2D>().enabled = false;
        doorOpen.Play();
    }
}
