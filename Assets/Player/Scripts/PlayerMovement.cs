using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;

    private float hInput;
    private float vInput;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");

        if (hInput != 0)
        {
            vInput = 0;
        } else
        {
            hInput = 0;
        }

        anim.SetFloat("moveX", hInput);
        anim.SetFloat("moveY", vInput);

        transform.Translate(new Vector2(1, 0) * Time.deltaTime * hInput * moveSpeed);
        transform.Translate(new Vector2(0, 1) * Time.deltaTime * vInput * moveSpeed);
    }


}
