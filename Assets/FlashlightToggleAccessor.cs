using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FlashlightToggleAccessor : MonoBehaviour
{
    //If flashlight is on --> hard mode; if flashlight is off --> easy mode
    private bool isFlashlightOn = true;
    [SerializeField] private InputActionAsset inputActions;

    //private static FlashlightToggleAccessor instance;

    private void Awake()
    {

        // Find all objects in the scene of the same type
        GameObject[] objectsWithSameName = GameObject.FindGameObjectsWithTag("FlashlightToggleAccess");  // Optional: Use a tag to filter if necessary

        foreach (GameObject obj in objectsWithSameName)
        {
            // Check if the object is not the current one (this object)
            if (obj != this.gameObject && obj.name == this.gameObject.name)
            {
                isFlashlightOn = obj.GetComponent<FlashlightToggleAccessor>().getFlashlight();

                // Destroy the duplicate object
                //Debug.Log("Destroying duplicate object: " + obj.name);
                Destroy(obj);
            }
        }

        // Ensure this object persists across scenes (if needed)
        DontDestroyOnLoad(gameObject);
        //Debug.Log("PersistentObject Initialized, myBool is: " + isFlashlightOn);

    }

    void OnEnable()
    {
        // Listen for scene load events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Remove the scene load listener when the object is disabled
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called every time a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Scene Loaded: " + scene.name + ", myBool is: " + isFlashlightOn);

        // Don't reset myBool when returning to Scene 1
        if (scene.name == "StartMenu")
        {
            // Check the state of myBool before doing anything
            //Debug.Log("Returning to Scene 1, myBool should be: " + isFlashlightOn);
        }
    }

    public void setFlashlight(bool flashBool)
    {
        isFlashlightOn = flashBool;
    }

    public bool getFlashlight()
    {
        return isFlashlightOn;
    }
}
