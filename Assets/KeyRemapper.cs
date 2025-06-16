using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class KeyRebindManager : MonoBehaviour
{
    [Header("Rebind Settings")]
    public InputActionReference bribeAction;
    public TMP_Text bribeText;

    [Header("Action Map Reference")]
    public InputActionAsset playerControls; // Drag your PlayerControls asset here

    [Header("Display Settings")]
    [SerializeField] private string waitingText = "Waiting for input...";

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private string originalBindingName;

    private void Start()
    {
        // Debug: Check if action is properly set up
        if (bribeAction == null)
        {
            return;
        }

        if (bribeAction.action == null)
        {
            return;
        }


        // Store the original binding display name
        originalBindingName = bribeAction.action.GetBindingDisplayString();
        UpdateBindingText(originalBindingName);
    }

    private void OnDestroy()
    {
        // Clean up the rebinding operation if it's still running
        rebindingOperation?.Dispose();
    }

    public void StartRebinding()
    {
        if (bribeAction == null || bribeAction.action == null)
        {
            return;
        }

        // Make sure the action is enabled
        if (!bribeAction.action.enabled)
        {
            bribeAction.action.Enable();
        }

        // Disable the action temporarily during rebinding
        bribeAction.action.Disable();

        // Update UI to show waiting state
        UpdateBindingText(waitingText);

        // Start the rebinding operation
        rebindingOperation = bribeAction.action
            .PerformInteractiveRebinding(0) // Target first binding
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding("<Keyboard>/escape") // Don't allow escape as a binding
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => OnRebindComplete())
            .OnCancel(operation => OnRebindCancel())
            .Start();

    }

    private void OnRebindComplete()
    {

        // Apply the rebinding to the entire action asset to ensure consistency
        if (playerControls != null)
        {
            // Save the current overrides
            string rebindings = bribeAction.action.SaveBindingOverridesAsJson();

            // Apply to the action asset
            playerControls.LoadBindingOverridesFromJson(rebindings);
        }

        // Get the new binding display name
        string newBindingName = bribeAction.action.GetBindingDisplayString();
        UpdateBindingText(newBindingName);

        // Debug: Print all bindings after rebind
        for (int i = 0; i < bribeAction.action.bindings.Count; i++)
        {
            var binding = bribeAction.action.bindings[i];
        }

        // Clean up and re-enable
        CleanupRebinding();
    }

    private void OnRebindCancel()
    {

        // Restore original binding text
        UpdateBindingText(originalBindingName);

        // Clean up and re-enable
        CleanupRebinding();
    }

    private void CleanupRebinding()
    {
        // Dispose of the rebinding operation
        rebindingOperation?.Dispose();
        rebindingOperation = null;

        // Re-enable the action
        bribeAction.action.Enable();
    }

    private void UpdateBindingText(string text)
    {
        if (bribeText != null)
        {
            bribeText.text = text;
        }
    }

    /*// Test method to check if the action is working
    private void Update()
    {
        if (bribeAction != null && bribeAction.action != null && bribeAction.action.WasPressedThisFrame())
        {
            Debug.Log("Bribe action triggered!");
        }
    }*/
}