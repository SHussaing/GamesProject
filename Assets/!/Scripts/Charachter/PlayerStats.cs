using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public Animator animator;
    public PlayerHUD hud;

    private void Start()
    {
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


}
