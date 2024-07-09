using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일정시간까지 생존 클리어 조건
/// </summary>
public class SurvivalTimeLimitCondition : ClearCondition
{
    public override string SetDescription()
    {
        return "Survival Time Limit";
    }

    public override int SetClearCondition()
    {
        return 10;
    }

    public override bool IsCleared()
    {
        return GameManager.Instance.currentSurvivalTime >= SetClearCondition();
    }
}
