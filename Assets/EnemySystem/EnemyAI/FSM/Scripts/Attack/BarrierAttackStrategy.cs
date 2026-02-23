using UnityEngine;
using System.Collections.Generic;

public class BarrierAttackStrategy : IAttackStrategy
{
    private LayerMask enemyLayer;
    private float range;

    public BarrierAttackStrategy(LayerMask enemyLayer, float range)
    {
        this.enemyLayer = enemyLayer;
        this.range = range;
    }

    public void Attack(Enemy enemy)
    {
        if (enemy.IsDead) return;

        WizardEnemy wizard = enemy as WizardEnemy;
        if (wizard == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            enemy.transform.position,
            range,
            enemyLayer
        );

        List<Enemy> candidates = new List<Enemy>();

        foreach (var hit in hits)
        {
            Enemy other = hit.GetComponent<Enemy>();

            if (other != null &&
                other != enemy &&
                !other.IsDead)
            {
                candidates.Add(other);
            }
        }

        if (candidates.Count == 0) return;

        Enemy target =
            candidates[Random.Range(0, candidates.Count)];

        wizard.ActivateBarrier(target);
    }

    public bool IsInRange(Enemy enemy)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            enemy.transform.position,
            range,
            enemyLayer
        );

        foreach (var hit in hits)
        {
            Enemy other = hit.GetComponent<Enemy>();

            if (other != null &&
                other != enemy &&
                !other.IsDead)
            {
                return true;
            }
        }

        return false;
    }
}