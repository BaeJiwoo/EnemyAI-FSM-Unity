using UnityEngine;

public class BarrierFollower : MonoBehaviour
{
    private Enemy target;
    public Vector3 offset = new Vector3(0f, 1f, 0f);

    public void SetTarget(Enemy newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null || target.IsDead)
        {
            gameObject.SetActive(false);
            return;
        }

        transform.position = target.transform.position + offset;
    }
}