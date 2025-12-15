using UnityEngine;

public class PlayerrAttack : MonoBehaviour
{
    public int damage = 1;
    public float attackRange = 1.2f;
    public LayerMask enemyLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
        }
    }

    void Attack()
    {
        Collider2D enemyHit = Physics2D.OverlapCircle(
            transform.position,
            attackRange,
            enemyLayer
        );

        if (enemyHit != null)
        {
            AdvancedEnemy enemy = enemyHit.GetComponent<AdvancedEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
