using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] private List<Stage> stages = new List<Stage>();

    public int currentKillenemies;
    public int clearKillenemies;

    public Camera mainCamera;

    public Transform curPlayer;

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

    private void Update()
    {
        // Test
        if (Input.GetKeyDown(KeyCode.K))
        {
            LevelManager.Instance.ShowItemCard();
            TimeManager.Instance.TimeStop();
        }
    }

    private void CheckStageClear()
    {

    }
}