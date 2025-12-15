using UnityEngine;
using System.Collections;

public class AdvancedEnemy : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead;

    [Header("Attack")]
    public int damage = 20;
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;
    public LayerMask playerLayer;

    [Header("Flash")]
    public Color damageColor = Color.red;
    public float flashDuration = 0.15f;

    private float nextAttackTime;
    private SpriteRenderer sr;
    private Color originalColor;
    private Animator anim;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        originalColor = sr.color;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        if (Time.time >= nextAttackTime)
            TryAttackPlayer();
    }

    // ================= ENEMY ATTACK =================
    void TryAttackPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            attackRange,
            playerLayer
        );

        if (hit != null)
        {
            PlayerHealth player = hit.GetComponent<PlayerHealth>();
            if (player != null)
            {
                Debug.Log("ENEMY HIT PLAYER");
                player.TakeDamage(damage);
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    // ================= TAKE DAMAGE =================
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("ENEMY HP: " + currentHealth);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator FlashRed()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
    }

    // ================= DIE =================
    void Die()
    {
        isDead = true;

        // Stop attacking
        GetComponent<Collider2D>().enabled = false;

        // Play death animation
        anim.SetTrigger("Die");

        // Destroy AFTER animation
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        // wait for animation length (adjust if needed)
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
