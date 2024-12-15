using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgainButton : MonoBehaviour
{
    private string currentSceneName;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        Time.timeScale = 1.0f;
    }
    public void tryAgain()
    {
        Debug.Log("Function Called");
        SceneManager.LoadScene(currentSceneName);
    }
}