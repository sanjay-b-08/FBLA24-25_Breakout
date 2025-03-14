using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DifficultyModeController : MonoBehaviour
{
    public Light2D globalLight;
    public Light2D playerFlashlight;
    public Light2D playerLight;

    private FlashlightToggleAccessor flashlightToggleAccessor;

    //private bool isDifficultyHard;

    private void Awake()
    {
        flashlightToggleAccessor = GameObject.Find("FlashlightToggleAccess").GetComponent<FlashlightToggleAccessor>();
        //isDifficultyHard = flashlightToggleAccessor.getFlashlight();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (flashlightToggleAccessor != null)
        {
            if (flashlightToggleAccessor.getFlashlight() == false)
            {
                globalLight.intensity = 1;
                playerFlashlight.gameObject.SetActive(false);
                playerLight.gameObject.SetActive(false);
            }
            else
            {
                globalLight.intensity = 0.015f;
                playerFlashlight.gameObject.SetActive(true);
                playerLight.gameObject.SetActive(true);
            }
        }
    }

}
