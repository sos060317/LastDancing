using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInformation : MonoBehaviour
{
    public static StageInformation Instance;

    public int stageNumber;
    public string stageName;
    public ClearCondition clearCondition;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 스테이지 설정
    /// </summary>
    /// <param name="selectStage"></param>
    public void SetStageInformation(Stage selectStage)
    {
        stageNumber = selectStage.stageNumber;
        stageName = selectStage.stageName;
        clearCondition = selectStage.clearCondition;
    }
}