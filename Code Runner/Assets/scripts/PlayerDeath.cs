using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject deathEffect;
    public float respawnDelay = 1f;

    private PlayerStats playerStats;
    private bool isDead = false;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (!isDead && playerStats != null && playerStats.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // تأثير الموت
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // إخفاء اللاعب
        GetComponent<SpriteRenderer>().enabled = false;

        // تعطيل الحركة
        GetComponent<PlayerController>().enabled = false;

        // إعادة اللاعب بعد تأخير
        Invoke("Respawn", respawnDelay);
    }

    void Respawn()
    {
        // البحث عن CheckpointManager
        CheckpointManager cm = FindObjectOfType<CheckpointManager>();

        if (cm != null)
        {
            // إعادة اللاعب لآخر Checkpoint
            cm.RespawnPlayer(gameObject);

            Debug.Log("Respawned at checkpoint");
        }
        else
        {
            // لو مش لاقي CheckpointManager، ارجع لبداية المشهد
            transform.position = Vector3.zero;
            Debug.Log("No checkpoint found. Respawning at start.");
        }

        // إعادة تفعيل اللاعب
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<PlayerController>().enabled = true;

        // إعادة تعيين الصحة
        if (playerStats != null)
        {
            playerStats.health = 3;
        }

        isDead = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
     
        Debug.Log("Player touched: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.CompareTag("DeathZone"))
        {
            Debug.Log("DeathZone detected! Player should die.");
            Die();
        }
    }
}