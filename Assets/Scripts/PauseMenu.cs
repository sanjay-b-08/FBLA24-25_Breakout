using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    private bool isPaused;

    public FlashlightScript fs;

    //Input System Initialization
    public InputActions playerInputActions;
    private InputAction pauseAction;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Awake()
    {
        playerInputActions = new InputActions();
    }
    private void OnEnable()
    {
        pauseAction = playerInputActions.PlayerControls.Pause;
        pauseAction.Enable();
        pauseAction.performed += Pause;
    }
    private void OnDisable()
    {
        pauseAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseAction.IsPressed())
        {
            if (isPaused)
            {
                Application.Quit();
                Debug.Log("Quitting Game...");
            } else
            {
                pauseMenu.SetActive(true);
                PauseGame();
                Cursor.visible = true;
                fs.canControlLight = false;
            }
        } else if (Input.GetKeyDown(KeyCode.Space) && isPaused)
        {
            ResumeGame();
        } else if (Input.GetKeyDown(KeyCode.M) && isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        fs.canControlLight = true;
    }

    private void Pause(InputAction.CallbackContext ctx)
    {
        return;
    }
}
