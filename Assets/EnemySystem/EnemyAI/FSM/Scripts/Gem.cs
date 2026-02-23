using UnityEngine;

public class Gem : MonoBehaviour
{
    public int maxHp = 100;
    private int currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);

        Debug.Log("Gem HP : " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Gem Destroyed!");
        // 필요하면 이펙트 추가
        //Destroy(gameObject);
    }
}