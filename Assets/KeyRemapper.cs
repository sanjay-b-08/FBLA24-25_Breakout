using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeyRemapper : MonoBehaviour
{
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

    private void Start()
    {
        // Initialize binding displays
        InitializeBindingDisplay(bribeAction, bribeText);
        InitializeBindingDisplay(killAction, killText);

        // Initialize composite binding displays (assuming 2D Vector composite)
        InitializeCompositeBindingDisplay(movementAction, moveForwardText, "up");
        InitializeCompositeBindingDisplay(movementAction, moveLeftText, "left");
        InitializeCompositeBindingDisplay(movementAction, moveBackText, "down");
        InitializeCompositeBindingDisplay(movementAction, moveRightText, "right");
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
        // Apply the rebinding to the entire action asset
        if (playerControls != null && currentRebindingAction != null)
        {
            string rebindings = currentRebindingAction.action.SaveBindingOverridesAsJson();
            playerControls.LoadBindingOverridesFromJson(rebindings);
        }

        // Get the new binding display name
        if (currentRebindingAction != null && currentRebindingText != null)
        {
            string newBindingName = currentRebindingAction.action.GetBindingDisplayString(currentBindingIndex);
            currentRebindingText.text = newBindingName;
        }

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
        ResetBinding(bribeAction, bribeText);
        ResetBinding(killAction, killText);
        ResetBinding(movementAction, moveForwardText);
        ResetBinding(movementAction, moveLeftText);
        ResetBinding(movementAction, moveBackText);
        ResetBinding(movementAction, moveRightText);
    }

    private void ResetBinding(InputActionReference actionRef, TMP_Text textUI)
    {
        if (actionRef != null && actionRef.action != null && textUI != null)
        {
            actionRef.action.RemoveAllBindingOverrides();
            string resetBindingName = actionRef.action.GetBindingDisplayString();
            textUI.text = resetBindingName;
        }
    }
}