using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Transform respawnPoint;
    public float respawnDelay = 5f;

    [Header("UI")]
    public HealthBarUI healthBar;

    [Header("Flash")]
    public Color damageColor = Color.red;
    public float flashDuration = 0.15f;

    private SpriteRenderer sr;
    private Color originalColor;
    private bool isDead;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Start()
    {
        currentHealth = maxHealth;

        // 🔴 INITIALIZE HEALTH BAR
        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        StartCoroutine(FlashRed());

        // 🔴 UPDATE HEALTH BAR
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator FlashRed()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // 🔴 UPDATE HEALTH BAR
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
    }

    public bool IsFullHealth()
    {
        return currentHealth >= maxHealth;
    }

    void Die()
    {
        isDead = true;
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        currentHealth = maxHealth;
        transform.position = respawnPoint.position;
        sr.color = originalColor;
        isDead = false;

        // 🔴 RESET HEALTH BAR
        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
    }
}
