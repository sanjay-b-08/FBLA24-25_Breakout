using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DifficultyModeController : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D playerFlashlight;
    public Light2D playerLight;

    private bool isDifficultyHard;
    // Start is called before the first frame update
    void Start()
    {
        isDifficultyHard = true;

        if (!isDifficultyHard)
        {
            globalLight.intensity = 1;
            playerFlashlight.gameObject.SetActive(false);
            playerLight.gameObject.SetActive(false);
        }
    }
}
