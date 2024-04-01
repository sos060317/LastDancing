using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    // 나중에 SerializeField 삭제
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxhealth;
    
    [HideInInspector] public Action onDie;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = value;

            currentHealth = Mathf.Min(currentHealth, maxhealth);

            if (currentHealth <= 0)
            {
                onDie?.Invoke();
            }
        }
    }

    protected void Start()
    {
        currentHealth = maxhealth;
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(currentHealth, 0, maxhealth);
    }
}
