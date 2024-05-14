using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject CanvasGame;
    public GameObject CanvasUpgrade;
    public GameObject Shop;
    public Transform Player;
    public Text ShopPrompt;
    [SerializeField] GameObject MenuCoins;
    private Text Coins;
    private PlayerStats playerStats;


    private void Start()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        Coins = MenuCoins.GetComponent<Text>();
    }

    void Update() 
    {
        //if player is near the shop
        if (Vector3.Distance(Player.position, Shop.transform.position) < 3)
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
        Coins.text = playerStats.Coins.ToString();
        //disable game canvas
        CanvasGame.SetActive(false);
        //enable upgrade canvas
        CanvasUpgrade.SetActive(true);
        //enable cursor
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableUpgradeMenu() 
    { 
        //enable game canvas
        CanvasGame.SetActive(true);
        //disable upgrade canvas
        CanvasUpgrade.SetActive(false);
        //disable cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

}
