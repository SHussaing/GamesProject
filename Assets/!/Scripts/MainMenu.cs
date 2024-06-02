using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    private Canvas mainMenu;
    private Canvas credits;
    private Canvas instructions;

    private void Start()
    {
        //find all the canvases
        mainMenu = GameObject.Find("CanvasMenu").GetComponent<Canvas>();
        credits = GameObject.Find("CanvasCredits").GetComponent<Canvas>();
        instructions = GameObject.Find("CanvasInstructions").GetComponent<Canvas>();
        mainMenu.enabled = true;
        // Make the cursor visible
        Cursor.visible = true;
        // Ensure the cursor is not locked
        Cursor.lockState = CursorLockMode.None;
    }

    public void startGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadInstructions()
    {
        mainMenu.enabled = false;
        credits.enabled = false;

        instructions.enabled = true;
    }

    public void loadCredits()
    {
        mainMenu.enabled = false;
        instructions.enabled = false;

        credits.enabled = true;
    }

    public void backToMenu()
    {
        credits.enabled = false;
        instructions.enabled = false;

        mainMenu.enabled = true;
    }

    public void quitGame ()
    {
        Application.Quit();
    }
}
