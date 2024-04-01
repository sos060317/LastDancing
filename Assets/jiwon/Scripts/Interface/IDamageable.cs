public interface IDamageable
{
    float CurrentHealth { get; set; }
    public void TakeDamage(float amount);
}