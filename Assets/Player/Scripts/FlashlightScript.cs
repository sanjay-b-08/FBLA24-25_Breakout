using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightScript : MonoBehaviour
{
    Vector3 mousePosition;
    Vector3 worldPosition;

    public Light2D playerFlashlight;

    [HideInInspector] public bool canControlLight;
    // Start is called before the first frame update
    void Start()
    {
        mousePosition = Input.mousePosition;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        worldPosition.z = 0f;

        Cursor.visible = false;

        canControlLight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canControlLight)
        {
            mousePosition = Input.mousePosition;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            worldPosition.z = 0f;

            //these values  match up with the beam values
            float x = worldPosition.x;
            float y = worldPosition.y;
            float z = worldPosition.z;

            Vector2 direction = (worldPosition - transform.position);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            angle -= 90f;

            //Ensures that player flashlight follows mouse
            transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector2 flashlightDistance = worldPosition - transform.position;

            //Ensures that player flashlight follows the distance
            playerFlashlight.pointLightOuterRadius = flashlightDistance.sqrMagnitude;
            playerFlashlight.pointLightInnerRadius = flashlightDistance.sqrMagnitude - 5.4f;

            playerFlashlight.pointLightInnerAngle = flashlightDistance.sqrMagnitude * 4f;

            //Limits outer radius
            if (flashlightDistance.sqrMagnitude < 7.6f)
            {
                playerFlashlight.pointLightOuterRadius = 7.6f;
            } else if (flashlightDistance.sqrMagnitude > 12f)
            {
                playerFlashlight.pointLightOuterRadius = 12f;
            }

            //limits inner radius
            if (flashlightDistance.sqrMagnitude - 5.4f < 2.2f)
            {
                playerFlashlight.pointLightInnerRadius = 2.2f;
            }
            else if (flashlightDistance.sqrMagnitude - 5.4f > 4.13f)
            {
                playerFlashlight.pointLightInnerRadius = 4.13f;
            }

            //limits inner angle
            if (flashlightDistance.sqrMagnitude * 4f < 21)
            {
                playerFlashlight.pointLightInnerAngle = 21f;
            } else if (flashlightDistance.sqrMagnitude * 4f > 48)
            {
                playerFlashlight.pointLightInnerAngle = 48f;
            }
        }

    }
}
