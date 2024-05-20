using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject CanvasGame;
    public GameObject CanvasUpgrade;
    public Text ShopPrompt;
    private GameObject ShopKeeper;
    private Transform PlayerTransform;
    [SerializeField] GameObject MenuCoins;
    private AudioSource purchaseSound;
    private AudioSource errorSound;
    private Text ShopCoins;
    private Text ErrorText;
    private PlayerStats playerStats;
    private Throwing[] Attacks;
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;


    private void Start()
    {
        GetReferences();
        SavePlayer.loadStats(playerStats, playerMovement, playerDash, Attacks[0], Attacks[1]);
        setDefaultValues();

    }
    private void setDefaultValues()
    {
        // if scene is Level01 set default values for prefabs
        if (SceneManager.GetActiveScene().name == "Level01")
        {
            playerStats.maxHealth = 100;
            playerStats.health = 100;
            playerStats.Coins = 0;
            playerMovement.walkSpeed = 5;
            playerMovement.sprintSpeed = 8;
            playerMovement.jumpForce = 12;
            playerDash.dashCd = 12;
            //knife speed
            Attacks[0].throwCooldown = 0.65f;
            //grenade speed
            Attacks[1].throwCooldown = 15;

            //knife damage
            Attacks[0].objectToThrow.gameObject.GetComponent<Projectile>().damage = 50;
            //explosion radius and visual effect
            Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionRadius = 4;
            Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionEffect.GetComponent<ParticleSystem>().startSize = 24;
            //explosion damage
            Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionDamage = 120;
        }
    }

    private void GetReferences()
    {
        purchaseSound = GameObject.Find("PurchaseSound").GetComponent<AudioSource>();
        errorSound = GameObject.Find("ErrorSound").GetComponent<AudioSource>();
        ShopCoins = MenuCoins.GetComponent<Text>();
        ShopKeeper = GameObject.Find("ShopKeeper");
        PlayerTransform = GameObject.Find("Player").transform;
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        Attacks = GameObject.Find("Player").GetComponents<Throwing>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerDash = GameObject.Find("Player").GetComponent<PlayerDash>();
    }

    void Update() 
    {
        //if player is near the shop
        if (Vector3.Distance(PlayerTransform.position, ShopKeeper.transform.position) < 3)
        {
            ShopPrompt.enabled = true;
            if (Input.GetKeyDown(KeyCode.F))
            {
                ActivateUpgradeMenu();
            }
        }
        else
        {
            ShopPrompt.enabled = false;
            DisableUpgradeMenu();
        }
    }

    public void ActivateUpgradeMenu()
    {
        UpdateCoins();
        //disable game canvas
        CanvasGame.SetActive(false);
        //enable upgrade canvas
        CanvasUpgrade.SetActive(true);
        ErrorText = GameObject.Find("ErrorText").GetComponent<Text>();
        ErrorText.enabled = false;
        //disable player Attack
        Attacks[0].enabled = false;
        Attacks[1].enabled = false;
        //enable cursor
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableUpgradeMenu() 
    { 
        //enable game canvas
        CanvasGame.SetActive(true);
        //disable upgrade canvas
        CanvasUpgrade.SetActive(false);
        //enable player Attack
        Attacks[0].enabled = true;
        Attacks[1].enabled = true;
        //disable cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UpdateCoins()
    {
        playerStats.UpdateCoinsHUD();
        ShopCoins.text = playerStats.Coins.ToString();
    }

    private bool checkCoins(int price, bool upgradeLimit)
    {
        if (playerStats.Coins >= price && upgradeLimit)
        {
            playerStats.Coins -= price;
            purchaseSound.Play();
            UpdateCoins();
            return true;
        }
        else
        {
            ErrorTextDisplay();
            return false;
        }
    }

    private void ErrorTextDisplay()
    {
        //enable error text for 2 seconds then diable it
        ErrorText.enabled= true;
        Invoke("DisableErrorText", 2f);
        errorSound.Play();
    }
    private void DisableErrorText()
    {
        ErrorText.enabled = false;
    }

    public static void SavePlayerStats()
    {

    }


    private int healthPrice = 175;
    public void UpgradePlayerHealth()
    {
        if (checkCoins(healthPrice, playerStats.maxHealth != 500))
        {
            playerStats.maxHealth += 10;
            playerStats.health += 10;
            playerStats.CheckHealth();
            SavePlayer.saveStats(playerStats, playerMovement, playerDash, Attacks[0], Attacks[1]);
        }
    }

    private int speedPrice = 200;
    public void UpgradePlayerSpeed()
    {
        if (checkCoins(speedPrice, playerMovement.walkSpeed < 10f))
        {
            //increase the player speed by 5%
            playerMovement.walkSpeed = playerMovement.walkSpeed + (5 *  0.05f);
            playerMovement.sprintSpeed = playerMovement.sprintSpeed + (8 * 0.05f);
            SavePlayer.saveStats(playerStats, playerMovement, playerDash, Attacks[0], Attacks[1]);
        }
    }

    private int DashPrice = 275;
    public void UgradePlayerDash()
    {
        if (checkCoins(DashPrice, playerDash.dashCd != 3))
        {
            playerDash.dashCd -= 1;
            SavePlayer.saveStats(playerStats, playerMovement, playerDash, Attacks[0], Attacks[1]);
        }
    }

    private int KnifeDamagePrice = 170;
    public void UpgradeKnifeDamage()
    {
        if (checkCoins(KnifeDamagePrice, Attacks[0].objectToThrow.gameObject.GetComponent<Projectile>().damage < 220))
        {
            Attacks[0].objectToThrow.gameObject.GetComponent<Projectile>().damage += 10;
        }
    }

    private int KnifeSpeedPrice = 200;
    public void UpgradeKnifeSpeed()
    {
        if (checkCoins(KnifeSpeedPrice, Attacks[0].throwCooldown > 0.04f))
        {
            Attacks[0].throwCooldown -= 0.05f;
            SavePlayer.saveStats(playerStats, playerMovement, playerDash, Attacks[0], Attacks[1]);
        }
    }
    
    private int jumpPrice = 325;

    public void UpgradeJump()
    {
        if (checkCoins(jumpPrice, playerMovement.jumpForce < 20))
        {
            playerMovement.jumpForce += 1.25f;
            SavePlayer.saveStats(playerStats, playerMovement, playerDash, Attacks[0], Attacks[1]);
        }
    }

    private int grenadeCDPrice = 450;
    public void UpgradeGrenadeCD()
    {
        if (checkCoins(grenadeCDPrice, Attacks[1].throwCooldown != 4))
        {
            Attacks[1].throwCooldown -= 1f;
            SavePlayer.saveStats(playerStats, playerMovement, playerDash, Attacks[0], Attacks[1]);
        }
    }

    private int grenadeRadiusPrice = 650;
    public void UpgradeGrenadeRadius()
    {
        if (checkCoins(grenadeRadiusPrice, Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionRadius <= 10))
        {
            Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionRadius += 1f;
            Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionEffect.GetComponent<ParticleSystem>().startSize += 4f;
        }
    }

    private int grenadeDamagePrice = 500;
    public void UpgradeGrenadeDamage()
    {
        if (checkCoins(grenadeDamagePrice, Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionDamage <= 500))
        {
            Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionDamage += 30;
        }
    }


}
