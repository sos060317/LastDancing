using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllEnemiesCondition : ClearCondition
{
    public override bool IsCleared()
    {
        return GameManager.Instance.currentKillenemies >= GameManager.Instance.clearKillenemies;
    }
}
