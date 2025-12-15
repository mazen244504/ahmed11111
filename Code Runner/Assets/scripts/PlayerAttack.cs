using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage = 10f;
    public GameObject swordHitEffect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        Debug.Log("Player attacking!");

        if (swordHitEffect != null)
        {
            Instantiate(swordHitEffect, transform.position, Quaternion.identity);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Boss"))
            {
                // ابحث عن EnemyComplete بدل EnemyStats
                AdvancedEnemy boss = enemy.GetComponent<AdvancedEnemy>();
                if (boss != null)
                {
                    boss.TakeDamage((int)attackDamage);
                    Debug.Log("Hit Boss!");
                }
            }
            else if (enemy.CompareTag("Enemy"))
            {
                // هنا السطر المهم - غير EnemyStats لـ EnemyComplete
                AdvancedEnemy enemyComponent = enemy.GetComponent<AdvancedEnemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.TakeDamage((int)attackDamage);
                    Debug.Log("Hit Enemy!");
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}