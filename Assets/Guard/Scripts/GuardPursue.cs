using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class GuardPursue : MonoBehaviour
{
    public float moveSpeed;
    public Transform player;
    public float catchRange = 1f;

    private float distance;

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

        if (caught(transform.position))
        {
            Debug.Log("Player Caught!");
            //GameOver();
        }
    }

    private bool caught(Vector2 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, catchRange, playerMask) != null)
        {
            //Debug.Log("Player Caught!");
            return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 4);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRange);
    }
}
