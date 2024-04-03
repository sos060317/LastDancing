using System;
using UnityEngine;

/// <summary>
/// IDamageable 구현부분
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    // 나중에 SerializeField 삭제
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxhealth;
    
    [HideInInspector] public Action onDie;

    /// <summary>
    /// HP프로퍼티 구현부분
    /// </summary>
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

    /// <summary>
    /// 데미지 받기 구현부분
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(currentHealth, 0, maxhealth);
    }
}
