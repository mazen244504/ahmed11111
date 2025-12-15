using UnityEngine;

public class SimpleDamageFlash : MonoBehaviour
{
    public Color flashColor = Color.red;
    public float flashDuration = 0.2f;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    // استدع هذه الدالة من PlayerStats
    public void FlashOnDamage()
    {
        StartCoroutine(FlashEffect());
    }

    System.Collections.IEnumerator FlashEffect()
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
    }
}