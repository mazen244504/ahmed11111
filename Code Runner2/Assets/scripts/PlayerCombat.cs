using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Settings")]
    public int playerDamage = 1;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    [Header("Components")]
    public Transform attackPoint;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        // أنيميشن الهجوم
        if (anim != null)
            anim.SetTrigger("Attack");

        // الكشف عن الأعداء في المدى
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            AdvancedEnemy enemyScript = enemy.GetComponent<AdvancedEnemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(playerDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}