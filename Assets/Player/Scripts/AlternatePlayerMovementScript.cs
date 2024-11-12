using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class AlternatePlayerMovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Boolean moving;
    private Vector2 input;
    public float moveSpeed = 5f;

    public Animator anim;

    public LayerMask solids;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            input.x = UnityEngine.Input.GetAxisRaw("Horizontal");
            input.y = UnityEngine.Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                var tarPos = transform.position;
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

    private bool noCollision(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.3f, solids) != null)
        {
            return false;
        }
        return true;
    }
}
