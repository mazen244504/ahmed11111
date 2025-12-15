using UnityEngine;

public class AhmedHealthPickup : MonoBehaviour
{
    public int healAmount = 20;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null) return;

        if (!health.IsFullHealth())
        {
            health.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
