using UnityEngine;
using TMPro;

public class KeyRemapperUIConnector : MonoBehaviour
{
    [Header("UI Text References for this Scene")]
    public TMP_Text bribeText;
    public TMP_Text killText;
    public TMP_Text moveForwardText;
    public TMP_Text moveLeftText;
    public TMP_Text moveBackText;
    public TMP_Text moveRightText;

    private void Start()
    {
        // Wait a frame to ensure the singleton is initialized
        StartCoroutine(ConnectToSingleton());
    }

    private System.Collections.IEnumerator ConnectToSingleton()
    {
        // Wait until the KeyRemapper singleton is available
        while (KeyRemapper.Instance == null)
        {
            yield return null;
        }

        // Update the singleton's UI references with this scene's UI elements
        KeyRemapper.Instance.UpdateUIReferences(
            bribeText,
            killText,
            moveForwardText,
            moveLeftText,
            moveBackText,
            moveRightText
        );

        Debug.Log("Connected UI elements to KeyRemapper singleton");
    }

    // These methods can be called by UI buttons in this scene
    public void StartRebindingBribe()
    {
        KeyRemapper.Instance?.StartRebindingBribe();
    }

    public void StartRebindingKill()
    {
        KeyRemapper.Instance?.StartRebindingKill();
    }

    public void StartRebindingMoveForward()
    {
        KeyRemapper.Instance?.StartRebindingMoveForward();
    }

    public void StartRebindingMoveLeft()
    {
        KeyRemapper.Instance?.StartRebindingMoveLeft();
    }

    public void StartRebindingMoveBack()
    {
        KeyRemapper.Instance?.StartRebindingMoveBack();
    }

    public void StartRebindingMoveRight()
    {
        KeyRemapper.Instance?.StartRebindingMoveRight();
    }

    public void ResetAllBindings()
    {
        KeyRemapper.Instance?.ResetAllBindings();
    }

    public void SaveCurrentBindings()
    {
        KeyRemapper.Instance?.SaveCurrentBindings();
    }
}