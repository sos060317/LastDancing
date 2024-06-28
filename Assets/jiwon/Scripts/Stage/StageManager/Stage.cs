[System.Serializable]
public class Stage
{
    public int stageNumber;
    public string stageName;
    public bool isUnlock;
    public ClearCondition clearCondition;

    /// <summary>
    /// 스테이지 설정 생성자
    /// </summary>
    /// <param name="_stageNumber"></param>
    /// <param name="_isUnlock"></param>
    public Stage(int _stageNumber, bool _isUnlock, ClearCondition _clearCondition)
    {
        stageNumber = _stageNumber;
        stageName = "Stage " + _stageNumber;
        isUnlock = _isUnlock;
        clearCondition = _clearCondition;
    }

    /// <summary>
    /// 클리어 조건 확인
    /// </summary>
    /// <returns></returns>
    public bool IsStageCleared()
    {
        return clearCondition.IsCleared();
    }

    /// <summary>
    /// 스테이지 이름 반환
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return stageName;
    }
}
