using UnityEngine;

public class Enemy : MonoBehaviour, IAttackStats
{
    [Header("References")]
    public Transform target;
    public Rigidbody2D rb;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Combat Stats")]
    [SerializeField] private float baseAttackRange = 1.2f;
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float baseAttackCooldown = 1f;

    [Header("Obstacle Check")]
    public float obstacleCheckDistance = 0.5f;
    public LayerMask obstacleLayer;

    [Header("Attack Raycast")]
    public LayerMask attackLayer;

    public StateMachine stateMachine = new StateMachine();

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private int currentHp;
    private bool isDead = false;

    protected IAttackStrategy attackStrategy;

    public bool IsDead => isDead;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target").transform;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        currentHp = 50;

        attackStrategy = new RaycastAttackStrategy();

        stateMachine.ChangeState(new ChaseState(this, animator));
    }

    void Update()
    {
        stateMachine.Update();
    }

    // =========================
    // ⭐ Attack Stats 계산
    // =========================

    public int GetDamage()
    {
        return baseDamage; // 여기서 버프/디버프 계산 가능
    }

    public float GetAttackRange()
    {
        return baseAttackRange;
    }

    public float GetAttackCooldown()
    {
        return baseAttackCooldown;
    }

    // =========================
    // Movement
    // =========================

    public void MoveToTarget()
    {
        if (isDead) return;

        Vector2 dir = (target.position - transform.position).normalized;

        if (IsBlocked(dir))
        {
            Stop();
            return;
        }

        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);

        if (dir.x > 0.05f)
            spriteRenderer.flipX = false;
        else if (dir.x < -0.05f)
            spriteRenderer.flipX = true;
    }

    bool IsBlocked(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) < 0.01f)
            return false;

        Vector2 rayDir = new Vector2(Mathf.Sign(dir.x), 0f);
        Vector2 origin = col.bounds.center;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            rayDir,
            obstacleCheckDistance,
            obstacleLayer
        );

        return hit.collider != null;
    }

    public void Stop()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    // =========================
    // Strategy Delegation
    // =========================

    public virtual void Attack()
    {
        attackStrategy?.Attack(this);
        animator.SetTrigger("attackTrigger");
    }

    public bool IsInAttackRange()
    {
        return attackStrategy?.IsInRange(this) ?? false;
    }

    public Vector2 GetCenter()
    {
        return col.bounds.center;
    }

    public Vector2 GetFacingDirection()
    {
        return spriteRenderer.flipX ? Vector2.left : Vector2.right;
    }
}