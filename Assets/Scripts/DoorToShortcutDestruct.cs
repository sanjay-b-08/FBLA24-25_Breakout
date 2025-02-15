using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorToShortcutDestruct : MonoBehaviour
{
    //public Tilemap doorToShortcut;
    // Start is called before the first frame update
    public void shortcutDestruct()
    {
        gameObject.SetActive(false);
    }
}
