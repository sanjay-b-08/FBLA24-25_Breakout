using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//THIS SCRIPT DOESNT CARRY ON OVER TO THE NEXT SCREEN

public class KeyRemapper : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction = null;

    [SerializeField] private InputActionAsset playerInputActions;
    
    [SerializeField] TMP_Text pauseControlText;
 

    private int x = 1;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private void Awake()
    {

        // Find all objects in the scene of the same type
        GameObject[] objectsWithSameName = GameObject.FindGameObjectsWithTag("KeyRemapperManager");  // Optional: Use a tag to filter if necessary

        foreach (GameObject obj in objectsWithSameName)
        {
            // Check if the object is not the current one (this object)
            if (obj != this.gameObject && obj.name == this.gameObject.name)
            {
                Destroy(obj);
            }
        }

        // Ensure this object persists across scenes (if needed)
        DontDestroyOnLoad(gameObject);
        Debug.Log($"Object {gameObject.name} is the same instance as reference (Memory ID: {gameObject.GetInstanceID()})");
        //Debug.Log("PersistentObject Initialized, myBool is: " + isFlashlightOn);

    }

    //MAKE IT SO THAT THE BINDINGS ARE LOADED ONCE YOU GET ONTO THE GAME SCENE ONCE (NOT ON UPDATE)

    void Start()
    {
        // Load saved rebinds from PlayerPrefs
        LoadBindings();


        // Find the action
        InputAction pauseActionBind = playerInputActions.FindAction("Pause"); //GENERALIZE THIS FOR ALL THE PLAUSIBLE REBINDS
        if (pauseActionBind != null)
        {
            // Get the currently bound key (AFTER rebinding)
            pauseControlText.SetText(pauseActionBind.GetBindingDisplayString());
            //Debug.Log($"Action '{pauseActionBind}' is currently bound to: {keyName}");
        }
        else
        {
            Debug.LogWarning($"Action '{pauseActionBind}' not found.");
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (PlayerPrefs.GetInt("BindingsLoaded", 0) == 0) // 0 means not loaded yet
            {
                LoadBindings();
                Debug.Log($"Bindings set in Game Scene {x} times");
                x++;
                PlayerPrefs.SetInt("BindingsLoaded", 1); // Mark bindings as loaded
                PlayerPrefs.Save(); // Save the PlayerPrefs to persist the flag

                //Debug.Log("Loading rebinds from PlayerPrefs: " + PlayerPrefs.GetString("rebinds", ""));

            }
        }

        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            if (PlayerPrefs.GetInt("BindingsLoaded", 0) == 1) // 0 means not loaded yet
            {
                LoadBindings();
                //Debug.Log($"Bindings set in Game Scene {x} times");
                //x++;
                PlayerPrefs.SetInt("BindingsLoaded", 0); // Mark bindings as loaded
                PlayerPrefs.Save(); // Save the PlayerPrefs to persist the flag

                //Debug.Log("Loading rebinds from PlayerPrefs: " + PlayerPrefs.GetString("rebinds", ""));

            }
        }
    }

    public void StartPauseRebind()
    {
        pauseControlText.SetText("Waiting For Input...");

        rebindingOperation = pauseAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => CompleteRebind(pauseControlText, pauseAction))
            .Start();
    }

    private void CompleteRebind(TMP_Text text, InputActionReference referenceAction)
    {
        int bindingIndex = referenceAction.action.GetBindingIndexForControl(referenceAction.action.controls[0]);

        text.SetText(InputControlPath.ToHumanReadableString(referenceAction.action.bindings[bindingIndex].effectivePath, 
            InputControlPath.HumanReadableStringOptions.OmitDevice));

        SaveBindings();

        rebindingOperation.Dispose();
    }

    public void SaveBindings()
    {
        string rebinds = playerInputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();
    }

    public void LoadBindings()
    {
        string savedRebinds = PlayerPrefs.GetString("rebinds", "");
        Debug.Log("Loading rebinds from PlayerPrefs: " + savedRebinds);

        if (!string.IsNullOrEmpty(savedRebinds))
        {
            playerInputActions.LoadBindingOverridesFromJson(savedRebinds);
            Debug.Log("Bindings loaded successfully.");
        }
        else
        {
            Debug.LogWarning("No saved bindings found.");
        }
    }
}
