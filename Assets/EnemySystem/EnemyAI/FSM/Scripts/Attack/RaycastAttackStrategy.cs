using UnityEngine;

public class RaycastAttackStrategy : IAttackStrategy
{
    public void Attack(Enemy enemy)
    {
        if (enemy.IsDead) return;

        Vector2 origin = enemy.GetCenter();
        Vector2 dir = enemy.GetFacingDirection();

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            dir,
            enemy.GetAttackRange(),
            enemy.attackLayer
        );

        if (hit.collider != null)
        {
            Gem gem = hit.collider.GetComponent<Gem>();

            if (gem != null)
            {
                gem.TakeDamage(enemy.GetDamage());
                Debug.Log("Strategy Hit with Calculated Damage!");
                
            }
        }
    }

    public bool IsInRange(Enemy enemy)
    {
        Vector2 origin = enemy.GetCenter();
        Vector2 dir = enemy.GetFacingDirection();

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            dir,
            enemy.GetAttackRange(),
            enemy.attackLayer
        );

        return hit.collider != null;
    }
}