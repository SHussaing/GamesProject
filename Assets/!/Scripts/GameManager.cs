using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private PlayerCam PlayerCam;
    private Throwing[] Attacks;
    [SerializeField] GameObject PNLDeath;
    private UpgradeManager upgradeManager;
    bool isDead = false;


    public Vector3 spawn;

    // Start is called before the first frame update
    void Start()
    {
        GetReferences();
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        // Make the cursor invisible
        Cursor.visible = false;
        //start time
        Time.timeScale = 1;
    }

    private void GetReferences() 
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        PlayerCam = GameObject.Find("Player").GetComponent<PlayerCam>();
        Attacks = GameObject.Find("Player").GetComponents<Throwing>();
        upgradeManager = GameObject.Find("GameUpgrades").GetComponent<UpgradeManager>();
        PNLDeath.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        managePlayer();
        /*CheckZombies();*/
    }

    // Manage player
    private void managePlayer()
    {
        //if player is dead
        if (playerStats.health <= 0 && !isDead)
        {
            //set isDead to true
            isDead = true;
            PNLDeath.SetActive(true);
            //disable upgrademanager
            upgradeManager.DisableUpgradeMenu();
            playerMovement.enabled = false;
            PlayerCam.enabled = false;
            //disable player Attack
            Attacks[0].enabled = false;
            Attacks[1].enabled = false;
            //enable cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //stop time
            Time.timeScale = 0;
            //set the coins to 0
            SavePlayer.saveCoins(0);
        }
        if(isDead)
        {
            //restart or go to main menu
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                RestartGame();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MainMenu();
            }
        }
        //if player fell down
        if (playerMovement.rb.position.y <= -100)
        {
            playerStats.TakeDamage(playerStats.maxHealth / 2);
            playerMovement.rb.position = spawn;
            playerMovement.rb.velocity = Vector3.zero;
        }
    }
    // determine if all zombies are dead
    public void CheckZombies()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            //go to next level after 10 seconds
            Invoke("NextLevel", 10);
        }
    }

    private void NextLevel()
    {
        int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
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
