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
        CheckHealth();
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
       // hud.UpdateCoinsText(Coins);
    }

    public void UpdateCoinsHUD()
    {
        CoinsText.text = Coins.ToString();
    }



}
