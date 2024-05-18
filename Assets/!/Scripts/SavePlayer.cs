using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayer : MonoBehaviour
{
    public static int maxHealth = 0;
    public static int coins = 0;
    public static float walkSpeed = 0;
    public static float sprintSpeed = 0;
    public static float dashCD = 0;
    public static float knifeCD = 0;
    public static float jumpForce = 0;
    public static float grenadeCD = 0;

    // save the stats of the player
    public static void saveStats(PlayerStats playerStats, PlayerMovement playerMovement, PlayerDash playerDash, Throwing knife, Throwing grenade)
    {
        maxHealth = playerStats.maxHealth;
        coins = playerStats.Coins;
        walkSpeed = playerMovement.walkSpeed;
        sprintSpeed = playerMovement.sprintSpeed;
        dashCD = playerDash.dashCd;
        knifeCD = knife.throwCooldown;
        jumpForce = playerMovement.jumpForce;
        grenadeCD = grenade.throwCooldown;
    }

    // load the stats of the player
    public static void loadStats(PlayerStats playerStats, PlayerMovement playerMovement, PlayerDash playerDash, Throwing knife, Throwing grenade)
    {
        if (maxHealth != 0) 
        { 
            playerStats.maxHealth = maxHealth; 
            playerStats.health = maxHealth; 
        }
        if(coins != 0) playerStats.Coins = coins;
        if(walkSpeed != 0) playerMovement.walkSpeed = walkSpeed;
        if(sprintSpeed != 0) playerMovement.sprintSpeed = sprintSpeed;
        if(dashCD != 0) playerDash.dashCd = dashCD;
        if(knifeCD != 0) knife.throwCooldown = knifeCD;
        if(jumpForce != 0) playerMovement.jumpForce = jumpForce;
        if(grenadeCD != 0) grenade.throwCooldown = grenadeCD;
    }
}
