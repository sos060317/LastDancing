using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 적을 처치 클리어 조건
/// </summary>
public class KillAllEnemiesCondition : ClearCondition
{
    public override string SetDescription()
    {
        return "Kill All Enemies";
    }

    public override int SetClearCondition()
    {
        return 10;
    }

    public override bool IsCleared()
    {
        Debug.Log("killall");

        return GameManager.Instance.currentKillCount >= SetClearCondition();
    }
}
