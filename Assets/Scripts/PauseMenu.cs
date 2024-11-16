using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    
    public GameObject pauseMenu;
    private bool isPaused;

    void Start() {
        pauseMenu.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) {
                Application.Quit();
                Debug.Log("Quitting Game...");
            } else {
                pauseMenu.SetActive(true);
                PauseGame();
            }
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            ResumeGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}