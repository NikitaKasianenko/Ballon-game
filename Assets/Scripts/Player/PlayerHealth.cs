using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int currentHealth { get; private set; }
    public int maxHealth { get; private set; }

    public event Action<int> OnHealthChanged;
    public event Action OnPlayerDied;

    public void Initialize(int health)
    {
        currentHealth = health;
        maxHealth = health;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnPlayerDied?.Invoke();
        }
    }

}
