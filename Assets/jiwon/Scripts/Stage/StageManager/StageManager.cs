using TMPro;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int stageCount;

    [SerializeField] private TMP_Text stageDescription;
    [SerializeField] private TMP_Dropdown stageDropdown;

    [SerializeField] private List<Stage> stages = new();
    [SerializeField] private List<ClearCondition> clearConditions = new();

    private void Start()
    {
        RunAllDerivedClasses();
        Initialization();
    }

    /// <summary>
    /// Dropdown에 스테이지 초기화
    /// </summary>
    private void Initialization()
    {
        for(int i = 0; i < stageCount; i++)
        {
            stages.Add(new Stage(i, false, clearConditions[i % clearConditions.Count]));
        }

        // 생성한 스테이지 옵션 추가
        SetDropdwonOptions(stages);

        // 처음 스테이지 초기화
        stageDescription.text = "Stage " + stages[0] + ": " + stages[0].clearCondition.SetDescription();
        StageInformation.Instance.SetStageInformation(stages[0]);

        // 스테이지 옵션 변경 확인
        stageDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(stageDropdown);
        });
    }

    /// <summary>
    /// 성한 스테이지 옵션 추가
    /// </summary>
    /// <param name="stages"></param>
    private void SetDropdwonOptions(List<Stage> stages)
    {
        List<string> options = new();

        foreach(Stage stage in stages)
        {
            options.Add(stage.ToString());
        }

        stageDropdown.ClearOptions();
        stageDropdown.AddOptions(options);
    }

    /// <summary>
    /// 스테이지 옵션 변경 확인
    /// </summary>
    /// <param name="dropdown"></param>
    private void DropdownValueChanged(TMP_Dropdown dropdown)
    {
        int selectedStageIndex = dropdown.value;
        Stage selectedStage = stages[selectedStageIndex];
        stageDescription.text = "Stage " + selectedStageIndex + ": " + selectedStage.clearCondition.SetDescription();

        StageInformation.Instance.SetStageInformation(stages[selectedStageIndex]);
    }

    /// <summary>
    /// ClearCondition를 상속받은 클래스 함수 실행
    /// </summary>
    private void RunAllDerivedClasses()
    {
        Type baseType = typeof(ClearCondition);
        List<Type> derivedTypes = FindAllDerivedTypes(baseType);

        foreach(Type type in derivedTypes)
        {
            ClearCondition instance = (ClearCondition)Activator.CreateInstance(type);
            clearConditions.Add(instance);
        }
    }

    /// <summary>
    /// ClearCondition를 상속받는 클래스 찾기
    /// </summary>
    /// <param name="baseType"></param>
    /// <returns></returns>
    private List<Type> FindAllDerivedTypes(Type baseType)
    {
        List<Type> derivedTypes = new();
        Assembly assembly = Assembly.GetExecutingAssembly();

        foreach(Type type in assembly.GetTypes())
        {
            if (type.IsSubclassOf(baseType))
            {
                derivedTypes.Add(type);
            }
        }

        return derivedTypes;
    }

    /// <summary>
    /// 변수 동기화
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting) // 데이터를 보내는 입장
        {
            stream.SendNext(stageDescription.text);
            stream.SendNext(stageDropdown.value);
        }
        else if(stream.IsReading) // 데이터를 받는 입장
        {
            stageDescription.text = (string)stream.ReceiveNext();
            stageDropdown.value = (int)stream.ReceiveNext();
        }
    }
}