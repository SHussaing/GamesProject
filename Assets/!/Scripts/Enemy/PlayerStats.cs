using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public PlayerHUD hud;

    private void Start()
    {
        InitVariables();
  
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        hud.UpdateHealthText(health, maxHealth);
    }


}
