using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;

public class AlternatePlayerMovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool moving;
    private Vector2 input;
    public float moveSpeed = 5f;

    public Animator anim;

    public AudioSource escapeSound;

    public LayerMask solids;
    public LayerMask cafDoor;
    public LayerMask recDoor;
    public LayerMask endDoor;

    public Tilemap doorToCaf;
    private DoorToCafDestruct dtc;

    //public Tilemap doorToRec;
    //private DoorToRecDestruct rtc;

    public Tilemap doorToEnd;
    private DoorToEndDestruct dte;
    void Start()
    {
        dtc = doorToCaf.GetComponent<DoorToCafDestruct>();
        //rtc = doorToRec.GetComponent<DoorToRecDestruct>();
        dte = doorToEnd.GetComponent<DoorToEndDestruct>();

    }

    // Update is called once per frame
    void Update()
    {
        cafDoorDestruct(transform.position);
        //recDoorDestruct(transform.position);
        endDoorDestruct(transform.position);

        if (!moving)
        {
            input.x = UnityEngine.Input.GetAxisRaw("Horizontal");
            input.y = UnityEngine.Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                Vector2 tarPos = transform.position;
                tarPos.x += input.x/2;
                tarPos.y += input.y/2;

                if (noCollision(tarPos))
                {
                    StartCoroutine(Move(tarPos));
                }
            }
        }



        if (input.x != 0)
        {
            input.y = 0;
        }

        anim.SetFloat("moveX", input.x);
        anim.SetFloat("moveY", input.y);

    }

    IEnumerator Move(Vector3 tarPos)
    {
        moving = true;

        while ((tarPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, tarPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = tarPos;

        moving = false;
    }

    //For collisions
    private bool noCollision(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.3f, solids) != null)
        {
            //Debug.Log("Collision Detected");
            return false;
        }
        return true;
    }

    private void cafDoorDestruct(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.4f, cafDoor) != null)
        {
            dtc.cafDestruct();
        }
    }

    /*private void recDoorDestruct(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.4f, recDoor) != null)
        {
            //rtc.recDestruct();
        }
    }*/

    private void endDoorDestruct(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.4f, endDoor) != null)
        {
            dte.endDestruct();
            escapeSound.Play();
        }
    }

}
