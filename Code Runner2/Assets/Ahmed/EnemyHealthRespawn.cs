using System.Collections;
using UnityEngine;

public class EnemyHealthRespawn : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;

    [Header("Respawn")]
    public float respawnDelay = 5f;
    public Transform respawnPoint;

    [Header("UI")]
    [SerializeField] private EnemyHealthBar healthBar;

    private int currentHealth;
    private Animator anim;
    private Collider2D col;
    private SpriteRenderer sr;
    private bool isDead;

    void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.Setup(maxHealth);
            healthBar.Hide();
        }

        if (respawnPoint == null)
        {
            GameObject p = new GameObject(name + "_RespawnPoint");
            p.transform.position = transform.position;
            respawnPoint = p.transform;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.Show();
            healthBar.UpdateHealth(currentHealth);
        }

        if (currentHealth <= 0)
            Die();
        else
            anim.SetTrigger("hurt");
    }

    void Die()
    {
        isDead = true;

        if (healthBar != null)
            healthBar.Hide();   // ✅ HIDE IMMEDIATELY ON DEATH

        anim.SetTrigger("die");
        col.enabled = false;

        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(
            anim.GetCurrentAnimatorStateInfo(0).length
        );

        sr.enabled = false;

        yield return new WaitForSeconds(respawnDelay);
        Respawn();
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;

        currentHealth = maxHealth;
        isDead = false;

        sr.enabled = true;
        col.enabled = true;
        anim.Play("idle");

        if (healthBar != null)
        {
            healthBar.UpdateHealth(maxHealth);
            healthBar.Hide();
        }
    }
}
