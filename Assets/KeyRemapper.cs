using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeyRemapper : MonoBehaviour
{
    //THE KEY REBINDS PERSIST FOR EACH PLAY THROUGH, DOES NOT RESET!!!!!!!

    // Singleton instance
    public static KeyRemapper Instance { get; private set; }

    [Header("Action References")]
    public InputActionReference bribeAction;
    public InputActionReference killAction;
    public InputActionReference movementAction;  // Single movement action with composite

    [Header("UI Text References")]
    public TMP_Text bribeText;
    public TMP_Text killText;
    public TMP_Text moveForwardText;
    public TMP_Text moveLeftText;
    public TMP_Text moveBackText;
    public TMP_Text moveRightText;

    [Header("Action Map Reference")]
    public InputActionAsset playerControls;

    [Header("Display Settings")]
    [SerializeField] private string waitingText = "Waiting for input...";

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private InputActionReference currentRebindingAction;
    private TMP_Text currentRebindingText;
    private string originalBindingName;
    private int currentBindingIndex = -1;

    // Key for saving binding overrides to PlayerPrefs
    private const string BINDING_OVERRIDES_KEY = "InputBindingOverrides";

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load saved binding overrides when singleton is first created
            LoadBindingOverrides();
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Only initialize if this is the singleton instance
        if (Instance == this)
        {
            InitializeAllBindingDisplays();
        }
    }

    private void InitializeAllBindingDisplays()
    {
        // Initialize binding displays after loading overrides
        InitializeBindingDisplay(bribeAction, bribeText);
        InitializeBindingDisplay(killAction, killText);

        // Initialize composite binding displays (assuming 2D Vector composite)
        InitializeCompositeBindingDisplay(movementAction, moveForwardText, "up");
        InitializeCompositeBindingDisplay(movementAction, moveLeftText, "left");
        InitializeCompositeBindingDisplay(movementAction, moveBackText, "down");
        InitializeCompositeBindingDisplay(movementAction, moveRightText, "right");
    }

    // Method to update UI references when switching scenes
    public void UpdateUIReferences(TMP_Text bribeText, TMP_Text killText,
                                 TMP_Text moveForwardText, TMP_Text moveLeftText,
                                 TMP_Text moveBackText, TMP_Text moveRightText)
    {
        this.bribeText = bribeText;
        this.killText = killText;
        this.moveForwardText = moveForwardText;
        this.moveLeftText = moveLeftText;
        this.moveBackText = moveBackText;
        this.moveRightText = moveRightText;

        // Refresh displays with new UI references
        InitializeAllBindingDisplays();
    }

    private void LoadBindingOverrides()
    {
        if (playerControls != null && PlayerPrefs.HasKey(BINDING_OVERRIDES_KEY))
        {
            string bindingOverrides = PlayerPrefs.GetString(BINDING_OVERRIDES_KEY);
            if (!string.IsNullOrEmpty(bindingOverrides))
            {
                playerControls.LoadBindingOverridesFromJson(bindingOverrides);
                Debug.Log("Loaded binding overrides from PlayerPrefs");
            }
        }
    }

    private void SaveBindingOverrides()
    {
        if (playerControls != null)
        {
            string bindingOverrides = playerControls.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(BINDING_OVERRIDES_KEY, bindingOverrides);
            PlayerPrefs.Save();
            Debug.Log("Saved binding overrides to PlayerPrefs");
        }
    }

    private void InitializeBindingDisplay(InputActionReference actionRef, TMP_Text textUI)
    {
        if (actionRef != null && actionRef.action != null && textUI != null)
        {
            string bindingName = actionRef.action.GetBindingDisplayString();
            textUI.text = bindingName;
        }
    }

    private void InitializeCompositeBindingDisplay(InputActionReference actionRef, TMP_Text textUI, string compositePart)
    {
        if (actionRef != null && actionRef.action != null && textUI != null)
        {
            int bindingIndex = GetCompositeBindingIndex(actionRef.action, compositePart);
            if (bindingIndex != -1)
            {
                string bindingName = actionRef.action.GetBindingDisplayString(bindingIndex);
                textUI.text = bindingName;
            }
        }
    }

    private int GetCompositeBindingIndex(InputAction action, string compositePart)
    {
        var bindings = action.bindings;
        for (int i = 0; i < bindings.Count; i++)
        {
            if (bindings[i].name.Equals(compositePart, System.StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1;
    }

    private void OnDestroy()
    {
        rebindingOperation?.Dispose();

        // Clear the singleton reference if this instance is being destroyed
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // Individual rebind methods
    public void StartRebindingBribe()
    {
        StartRebinding(bribeAction, bribeText, 0);
    }

    public void StartRebindingKill()
    {
        StartRebinding(killAction, killText, 0);
    }

    public void StartRebindingMoveForward()
    {
        int bindingIndex = GetCompositeBindingIndex(movementAction.action, "up");
        StartRebinding(movementAction, moveForwardText, bindingIndex);
    }

    public void StartRebindingMoveLeft()
    {
        int bindingIndex = GetCompositeBindingIndex(movementAction.action, "left");
        StartRebinding(movementAction, moveLeftText, bindingIndex);
    }

    public void StartRebindingMoveBack()
    {
        int bindingIndex = GetCompositeBindingIndex(movementAction.action, "down");
        StartRebinding(movementAction, moveBackText, bindingIndex);
    }

    public void StartRebindingMoveRight()
    {
        int bindingIndex = GetCompositeBindingIndex(movementAction.action, "right");
        StartRebinding(movementAction, moveRightText, bindingIndex);
    }

    private void StartRebinding(InputActionReference actionRef, TMP_Text textUI, int bindingIndex)
    {
        if (actionRef == null || actionRef.action == null || textUI == null || bindingIndex == -1)
        {
            Debug.LogError("Action reference, text UI is not assigned, or binding index is invalid!");
            return;
        }

        // Store references for the current rebinding
        currentRebindingAction = actionRef;
        currentRebindingText = textUI;
        currentBindingIndex = bindingIndex;
        originalBindingName = actionRef.action.GetBindingDisplayString(bindingIndex);

        // Disable the action temporarily during rebinding
        actionRef.action.Disable();

        // Update UI to show waiting state
        textUI.text = waitingText;

        // Start the rebinding operation for the specific binding
        rebindingOperation = actionRef.action
            .PerformInteractiveRebinding(bindingIndex) // Target specific binding
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => OnRebindComplete())
            .OnCancel(operation => OnRebindCancel())
            .Start();
    }

    private void OnRebindComplete()
    {
        // Get the new binding display name first
        if (currentRebindingAction != null && currentRebindingText != null)
        {
            string newBindingName = currentRebindingAction.action.GetBindingDisplayString(currentBindingIndex);
            currentRebindingText.text = newBindingName;
        }

        // Save the binding overrides to persistent storage
        SaveBindingOverrides();

        CleanupRebinding();
    }

    private void OnRebindCancel()
    {
        if (currentRebindingText != null)
        {
            currentRebindingText.text = originalBindingName;
        }

        CleanupRebinding();
    }

    private void CleanupRebinding()
    {
        rebindingOperation?.Dispose();
        rebindingOperation = null;

        if (currentRebindingAction != null && currentRebindingAction.action != null)
        {
            currentRebindingAction.action.Enable();
        }

        currentRebindingAction = null;
        currentRebindingText = null;
        currentBindingIndex = -1;
    }

    public void ResetAllBindings()
    {
        // Remove overrides from all actions
        if (playerControls != null)
        {
            foreach (var actionMap in playerControls.actionMaps)
            {
                foreach (var action in actionMap.actions)
                {
                    action.RemoveAllBindingOverrides();
                }
            }
        }

        // Clear saved overrides
        PlayerPrefs.DeleteKey(BINDING_OVERRIDES_KEY);
        PlayerPrefs.Save();

        // Update UI displays to show original bindings
        RefreshAllBindingDisplays();

        Debug.Log("Reset all bindings to defaults");
    }

    private void RefreshAllBindingDisplays()
    {
        InitializeBindingDisplay(bribeAction, bribeText);
        InitializeBindingDisplay(killAction, killText);
        InitializeCompositeBindingDisplay(movementAction, moveForwardText, "up");
        InitializeCompositeBindingDisplay(movementAction, moveLeftText, "left");
        InitializeCompositeBindingDisplay(movementAction, moveBackText, "down");
        InitializeCompositeBindingDisplay(movementAction, moveRightText, "right");
    }

    // Optional: Method to manually save current bindings (useful for settings menu)
    public void SaveCurrentBindings()
    {
        SaveBindingOverrides();
    }

    // Optional: Method to check if there are saved bindings
    public bool HasSavedBindings()
    {
        return PlayerPrefs.HasKey(BINDING_OVERRIDES_KEY);
    }
}