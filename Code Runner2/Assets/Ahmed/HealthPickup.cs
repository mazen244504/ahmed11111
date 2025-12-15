using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Range(1, 100)]
    public int healPercent = 25;

    public float respawnDelay = 5f;

    [Header("Effects")]
    public ParticleSystem pickupEffect;
    public GameObject healPopupPrefab;

    private SpriteRenderer sr;
    private Collider2D col;
    private Vector3 startPos;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player == null) return;

        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject popupObj = Instantiate(healPopupPrefab, canvas.transform);
        popupObj.transform.position =
            Camera.main.WorldToScreenPoint(transform.position);

        HealPopup popup = popupObj.GetComponent<HealPopup>();

        // 🔴 FULL HEALTH
        if (player.IsFullHealth())
        {
            popup.SetText("Health is already full", Color.yellow);
            return;
        }

        // 🟢 CALCULATE HEAL
        int healAmount = Mathf.RoundToInt(player.maxHealth * (healPercent / 100f));
        player.Heal(healAmount);

        // 🎨 PICK COLOR BASED ON PERCENT
        Color popupColor = Color.green;

        if (healPercent < 30)
            popupColor = Color.red;
        else if (healPercent <= 60)
            popupColor = Color.yellow;
        else
            popupColor = Color.green;

        popup.SetText("+" + healPercent + "% HP", popupColor);

        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        HidePickup();
        Invoke(nameof(Respawn), respawnDelay);
    }

    void HidePickup()
    {
        sr.enabled = false;
        col.enabled = false;
    }

    void Respawn()
    {
        sr.enabled = true;
        col.enabled = true;
        transform.position = startPos;
    }
}
