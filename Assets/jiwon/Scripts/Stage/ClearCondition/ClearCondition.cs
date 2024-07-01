[System.Serializable]   
public class ClearCondition
{
    /// <summary>
    /// 스테이지 설명 함수 선언
    /// </summary>
    /// <returns></returns>
    public virtual string SetDescription() => "";

    /// <summary>
    /// 스테이지 클리어 함수 선언
    /// </summary>
    /// <returns></returns>
    public virtual bool IsCleared() => true;
}
