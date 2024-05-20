using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerStats : CharacterStats
{
    public Animator animator;
    public PlayerHUD hud;
    public int Coins = 0;
    private Text CoinsText;

    private void Start()
    {
        CoinsText = GameObject.Find("Coins").GetComponent<Text>();
        // invoke the check health method
        Invoke("CheckHealth", 0.1f);
        Invoke("UpdateCoinsHUD", 0.1f);
    }


    public override void CheckHealth()
    {
        base.CheckHealth();
        hud.UpdateHealthText(health, maxHealth);
    }

    public override void Die()
    {
        base.Die();
        animator.SetTrigger("Die");
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        UpdateCoinsHUD();
        SavePlayer.saveCoins(Coins);
       // hud.UpdateCoinsText(Coins);
    }

    public void UpdateCoinsHUD()
    {
        CoinsText.text = Coins.ToString();
    }



}
