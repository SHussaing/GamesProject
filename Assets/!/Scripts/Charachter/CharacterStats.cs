using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    [SerializeField] public int health;
    [SerializeField] public int maxHealth;

    [SerializeField] protected bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        SetHealthTo(maxHealth);
    }

    public virtual void CheckHealth()
    {

        if (health <= 0)
        {

            health = 0;
            Die();
        }
        if (health >= maxHealth)
        {

            health = maxHealth;

        }
    }

    public void SetHealthTo(int healthToSetTo)
    {

        health = healthToSetTo;
        CheckHealth();

    }

    public void TakeDamage(int damage)
    {

        int healthAfterDamage = health - damage;
        SetHealthTo(healthAfterDamage);

    }

    public void Heal(int healAmount)
    {

        int healthAfterHeal = health + healAmount;
        SetHealthTo(healthAfterHeal);

    }
/*    public virtual void InitVariables()
    {

        maxHealth = 100;
        SetHealthTo(maxHealth);
        isDead = false;

    }*/

    public virtual void Die()
    {
        isDead = true;
    }

}
