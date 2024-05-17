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
    private Text ShopCoins;
    private PlayerStats playerStats;
    private Throwing[] Attacks;
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;


    private void Start()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        purchaseSound = GameObject.Find("PurchaseSound").GetComponent<AudioSource>();
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

    private bool checkCoins(int price)
    {
        if (playerStats.Coins >= price)
        {
            playerStats.Coins -= price;
            purchaseSound.Play();
            UpdateCoins();
            return true;
        }
        else
        {
            return false;
        }
    }


    private int healthPrice = 175;
    public void UpgradePlayerHealth()
    {
        if (checkCoins(healthPrice))
        {
            playerStats.maxHealth += 10;
            playerStats.health += 10;
            playerStats.CheckHealth();
        }
    }

    private int speedPrice = 200;
    public void UpgradePlayerSpeed()
    {
        if (checkCoins(speedPrice))
        {
            //increase the player speed by 5%
            playerMovement.walkSpeed = playerMovement.walkSpeed + (5 *  0.05f);
            playerMovement.sprintSpeed = playerMovement.sprintSpeed + (8 * 0.05f);
        }
    }

    private int DashPrice = 275;
    public void UgradePlayerDash()
    {
        if (checkCoins(DashPrice))
        {
            playerDash.dashCd -= 1;
        }
    }

    private int KnifeDamagePrice = 170;
    public void UpgradeKnifeDamage()
    {
        if (checkCoins(KnifeDamagePrice))
        {
            Attacks[0].objectToThrow.gameObject.GetComponent<Projectile>().damage += 5;
        }
    }

    private int KnifeSpeedPrice = 150;
    public void UpgradeKnifeSpeed()
    {
        if (checkCoins(KnifeSpeedPrice))
        {
            Attacks[0].throwCooldown -= 0.05f;
        }
    }
    
    private int jumpPrice = 325;

    public void UpgradeJump()
    {
        if (checkCoins(jumpPrice))
        {
            playerMovement.jumpForce += 1.25f;
        }
    }

    private int grenadeCDPrice = 450;
    public void UpgradeGrenadeCD()
    {
        if (checkCoins(grenadeCDPrice))
        {
            Attacks[1].throwCooldown -= 1f;
        }
    }

    private int grenadeRadiusPrice = 650;
    public void UpgradeGrenadeRadius()
    {
        if (checkCoins(grenadeRadiusPrice))
        {
            //default value = 4
            Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionRadius += 1f;
            //default value = 24
            Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionEffect.GetComponent<ParticleSystem>().startSize += 2f;
        }
    }

    private int grenadeDamagePrice = 500;
    public void UpgradeGrenadeDamage()
    {
        if (checkCoins(grenadeDamagePrice))
        {
            Attacks[1].objectToThrow.gameObject.GetComponent<Projectile>().explosionDamage += 30;
        }
    }


}
