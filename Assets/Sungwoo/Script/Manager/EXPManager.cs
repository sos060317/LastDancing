using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EXPManager : MonoBehaviour
{
    [SerializeField] private Image expGauge;
    [SerializeField] private TextMeshProUGUI expText;

    // 100Àº Å×½ºÆ®
    private int maxExp = 10;
    private int curExp = 0;

    #region ½Ì±ÛÅæ

    private static EXPManager instance = null;

    public static EXPManager Instance
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

    private void Start()
    {
        expText.text = curExp + " / " + maxExp;
    }

    private void Update()
    {
        expGauge.fillAmount = Mathf.Lerp(expGauge.fillAmount, (float)curExp / (float)maxExp, Time.deltaTime * 12f);
    }

    public void ExpPlus()
    {
        curExp++;

        // ·¹º§¾÷
        if (curExp >= maxExp)
        {
            curExp = 0;
            maxExp += 10;

            LevelManager.Instance.ShowItemCard();
            TimeManager.Instance.TimeStop();
        }

        expText.text = curExp + " / " + maxExp;
    }
}