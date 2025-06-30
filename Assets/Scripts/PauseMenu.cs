using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public bool isPaused;

    public bool isOnControlScreen;

    public FlashlightScript fs;

    //Input System Initialization
    public InputActions playerInputActions;
    private InputAction pauseAction;

    void Start()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
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
        /*if (pauseAction.IsPressed())
        {
            if (isPaused)
            {
                QuitGame();
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
            ToMainMenu();
        }*/

        if (pauseAction.IsPressed())
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        fs.canControlLight = false;
    }

    /*public void QuitGame()
    {
        Application.Quit();
        //Debug.Log("Quitting Game...");
    }*/

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        fs.canControlLight = true;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void Pause(InputAction.CallbackContext ctx)
    {
        return;
    }
}
