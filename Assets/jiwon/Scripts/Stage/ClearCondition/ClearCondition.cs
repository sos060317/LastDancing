using UnityEngine;

public abstract class ClearCondition : MonoBehaviour
{
    /// <summary>
    /// 스테이지 설명 함수 선언
    /// </summary>
    /// <returns></returns>
    public abstract string SetDescription();
    /// <summary>
    /// 스테이지 클리어 함수 선언
    /// </summary>
    /// <returns></returns>
    public abstract bool IsCleared();
}
