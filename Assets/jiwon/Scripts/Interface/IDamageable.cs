/// <summary>
/// HP 다운함수와 함수에서 사용할 HP프로퍼티 선언
/// </summary>
public interface IDamageable
{
    float CurrentHealth { get; set; }
    public void TakeDamage(float amount);
}