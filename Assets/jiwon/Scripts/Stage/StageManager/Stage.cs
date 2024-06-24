using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private int stageNumber;
    [SerializeField] private ClearCondition clearCondition;

    public bool IsStageCleared()
    {
        return clearCondition.IsCleared();
    }
}
