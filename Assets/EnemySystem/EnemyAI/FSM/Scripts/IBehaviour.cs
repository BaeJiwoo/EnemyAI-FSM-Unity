
public interface IAttackStrategy
{
    void Attack(Enemy enemy);
    bool IsInRange(Enemy enemy);
}

public interface IAttackStats
{
    int GetDamage();
    float GetAttackRange();
    float GetAttackCooldown();
}