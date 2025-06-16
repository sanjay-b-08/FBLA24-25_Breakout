using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebindControlAnimScript : MonoBehaviour
{
    public Animator rebindAnimator;

    public void appearAnimation()
    {
        rebindAnimator.SetBool("isOnControls", true);
    }

    public void disappearAnimatino()
    {
        rebindAnimator.SetBool("isOnControls", false);
    }
}
