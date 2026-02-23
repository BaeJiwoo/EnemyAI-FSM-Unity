using UnityEngine;
using System.Collections.Generic;

public class WizardEnemy : Enemy
{
    [Header("Barrier Settings")]
    public BarrierFollower barrier;   // 자식 오브젝트 연결
    public float barrierRange = 5f;
    public LayerMask enemyLayer;

    protected override void Start()
    {
        base.Start(); // ⭐ 반드시 호출

        // 기본 Raycast 전략 대신 Barrier 전략으로 교체
        SetBarrierStrategy();

        if (barrier != null)
            barrier.gameObject.SetActive(false);
    }

    void SetBarrierStrategy()
    {
        attackStrategy = new BarrierAttackStrategy(enemyLayer, barrierRange);
    }

    public void ActivateBarrier(Enemy target)
    {
        if (barrier == null) return;

        barrier.SetTarget(target);
        barrier.gameObject.SetActive(true);
    }
}