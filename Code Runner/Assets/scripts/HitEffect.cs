using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public GameObject hitParticles;
    public AudioClip hitSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayHitEffect(Vector3 position)
    {
        // تأثير الجسيمات
        if (hitParticles != null)
        {
            GameObject particles = Instantiate(hitParticles, position, Quaternion.identity);
            Destroy(particles, 1f);
        }

        // صوت الضربة
        if (hitSound != null && audioSource != null)
            audioSource.PlayOneShot(hitSound);

        // اهتزاز الكاميرا (اختياري)
        StartCoroutine(CameraShake(0.1f, 0.05f));
    }

    System.Collections.IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = originalPos;
    }
}