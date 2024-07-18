using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    
    public int currentKillCount;
    public int clearKillCount;

    public float currentSurvivalTime;
    public float clearSurvivalTime;

    public Transform gameClearHandler;
    public Transform gameOverHandler;

    public Camera mainCamera;

    public Transform curPlayer;

    public List<GameObject> players = new List<GameObject>();

    [SerializeField] private TMP_Text killText;
    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private TMP_Text inGameKillText;
    [SerializeField] private TMP_Text inGameSurvivalTimeText;
    public TextMeshProUGUI magazineText;

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
        InitializationStageClearCondition();
        Debug.Log(StageInformation.Instance.clearCondition.SetDescription());
        Debug.Log(players.Count);
    }

    private void Update()
    {
        // Test
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    LevelManager.Instance.ShowItemCard();
        //    TimeManager.Instance.TimeStop();
        //}

        CheckStageClear();
        SetScore();
    }

    private void InitializationStageClearCondition()
    {
        clearKillCount = StageInformation.Instance.clearCondition.SetClearCondition();
        clearSurvivalTime = StageInformation.Instance.clearCondition.SetClearCondition();
    }

    private void SetScore()
    {
        if (gameClearHandler.gameObject.activeSelf)
            return;

        currentSurvivalTime += Time.deltaTime;

        inGameKillText.text = "KILL: " + currentKillCount.ToString();
        inGameSurvivalTimeText.text = "Game Score: " + ((int)currentSurvivalTime).ToString();
    }

    /// <summary>
    /// 게임 클리어 조건 확인 함수
    /// </summary>
    private void CheckStageClear()
    {
        if(StageInformation.Instance.clearCondition.IsCleared())
        {
            TimeManager.Instance.TimeStop();

            killText.text = "KILL: " + currentKillCount.ToString();
            survivalTimeText.text = "Survival Time: " + ((int)currentSurvivalTime).ToString();

            gameClearHandler.gameObject.SetActive(true);
        }
    }

    public void CheckAllPlayerDie()
    {
        foreach (GameObject player in players)
        {
            if (player.transform.GetChild(0).gameObject.activeSelf)
                return;
        }

        RespawnManager.Instance.respwanBackground.gameObject.SetActive(false);
        gameOverHandler.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}