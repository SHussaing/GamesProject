using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private PlayerCam PlayerCam;
    private GameObject PNLDeath;

    public Vector3 spawn;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        PNLDeath = GameObject.Find("PNLDeath");
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        PlayerCam = GameObject.Find("Player").GetComponent<PlayerCam>();
        PNLDeath.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if player is dead
        if(playerStats.health <= 0)
        {
            PNLDeath.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            playerMovement.enabled = false;
            PlayerCam.enabled = false;
        }
        //if player fell down
        if(playerMovement.rb.position.y <= -100)
        {
            playerStats.TakeDamage(playerStats.maxHealth/2);
            playerMovement.rb.position = spawn;
            playerMovement.rb.velocity = Vector3.zero;
        }
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);  
    }
}
