using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    Vector3 mousePosition;
    Vector3 worldPosition;

    public bool canControlLight;
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


            //Debug.Log($"Mouse X Position: {x}, Mouse Y Position: {y}`, Mouse Z Position: {z}");
            //Debug.Log($"Beam X Position: {transform.position.x}, Beam Y Position: {transform.position.y}, Beam Z Position: {transform.position.z}");

            Vector2 direction = (worldPosition - transform.position);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            angle -= 90f;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

    }
}
