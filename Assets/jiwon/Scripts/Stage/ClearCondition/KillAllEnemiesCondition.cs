using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 적을 처치 클리어 조건
/// </summary>
public class KillAllEnemiesCondition : ClearCondition
{
    public override bool IsCleared()
    {
        Debug.Log("killall");

        return false;
    }
}
