using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class FlashlightToggleSettingsScript : MonoBehaviour
{
    // Start is called before the first frame update
    //[HideInInspector] public bool isFlashlightOn;
    public Sprite onSprite, offSprite;
    public Button toggleButton;

    public FlashlightToggleAccessor fta;

    private void Awake()
    {
        if (fta.getFlashlight() == true)
        {
            toggleButton.GetComponent<Image>().sprite = onSprite;
        }
        else
        {
            toggleButton.GetComponent<Image>().sprite = offSprite;
        }

    }
    /*void Start()
    {
        if (fta.getFlashlight() == true)
        {
            toggleButton.GetComponent<Image>().sprite = onSprite;
        }
        else
        {
            toggleButton.GetComponent<Image>().sprite = offSprite;
        }
    }*/

    public void flashlightToggleClick()
        {
        //fta.setFlashlight(!fta.getFlashlight());
        bool flashlightStatus = fta.getFlashlight();

            if (flashlightStatus == true)
            {
                toggleButton.GetComponent<Image>().sprite = offSprite;
                fta.setFlashlight(!fta.getFlashlight());
                //Debug.Log($"Bool Changed to: {fta.getFlashlight()}, should be FALSE");
            }
            else
            {
                toggleButton.GetComponent<Image>().sprite = onSprite;
                fta.setFlashlight(!fta.getFlashlight());
                //Debug.Log($"Bool Changed to: {fta.getFlashlight()}, should be TRUE");
            }
        }
    }
