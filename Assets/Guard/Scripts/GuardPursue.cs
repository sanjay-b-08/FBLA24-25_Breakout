using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GuardPursue : MonoBehaviour
{
    public float moveSpeed;
    public Transform player;

    private float distance;

    public LayerMask solids;
    public LayerMask playerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 4)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }
}
