using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일정시간까지 생존 클리어 조건
/// </summary>
public class SurvivalTimeLimitCondition : ClearCondition
{
    public override bool IsCleared()
    {
        Debug.Log("survivaltime");

        return false;
    }
}
