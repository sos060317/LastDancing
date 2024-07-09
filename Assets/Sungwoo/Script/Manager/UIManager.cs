using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image healthGauge;

    private float healthAmount = 1;

    #region ΩÃ±€≈Ê

    private static UIManager instance = null;

    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        healthGauge.fillAmount = Mathf.Lerp(healthGauge.fillAmount, healthAmount, Time.deltaTime * 12f);
    }

    public void HealthUpdate(float amount)
    {
        healthAmount = amount;
    }
}