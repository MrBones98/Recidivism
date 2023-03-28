using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float Health;

    [SerializeField]
    public float MaxHealth;

    private void Update()
    {
        if (Health <= 0)
        {
            Die();
        }
    }
    private void Awake()
    {
        Initialize();
    }
    public void Initialize() // to be called at the beginning of a lvl
    {
        Health = MaxHealth;
    }

    public float GetHealth()
    {
        return Health;
    }

    public void Heal(float healAmount)
    {
        Health += healAmount;
        if (Health > MaxHealth) Health = MaxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        //take damage animation
        Health -= damage;
        //print($"{gameObject.name} 's health is currently {Health}");
    }

    public void SetNewMaxHealth(float newMax)
    {
        MaxHealth = newMax;
        if (newMax <= 0)
        {
            Debug.LogError("WARNING: Max HP must be greater than zero!");
        }
    }

    protected virtual void Die()
    {
        print(gameObject.name + "died");
        Destroy(gameObject);
        //override for boss set healthbar again to false. !!
    }
}
