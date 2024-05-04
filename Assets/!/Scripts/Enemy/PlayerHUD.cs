using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHUD : MonoBehaviour
{

    [SerializeField] private ProgressBar healthBar;



    public void UpdateHealthText(int currentHealth , int MaxHealth)
    {
        healthBar.SetValues(currentHealth, MaxHealth);
    }

    
}
