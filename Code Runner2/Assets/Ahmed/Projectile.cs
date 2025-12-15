using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 3f;
    private int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Called by PlayerController after spawning
    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Find enemy health even if collider is on child
        EnemyHealthRespawn enemy = other.GetComponentInParent<EnemyHealthRespawn>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
