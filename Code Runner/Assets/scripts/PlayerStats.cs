using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 3;
    public int lives = 3;

    private float flickerTime = 0f;
    public float flickerDuration = 0.1f;

    private SpriteRenderer sr;

    public bool isImmune = false;
    private float immunityTime = 0f;
    public float immunityDuration = 1.5f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isImmune == true)
        {
            SpriteFlicker();
            immunityTime = immunityTime + Time.deltaTime;
            if (immunityTime >= immunityDuration)
            {
                isImmune = false;
                sr.enabled = true;
            }
        }
    }

    // دالة TakeDamage واحدة فقط!
    public void TakeDamage(int damage)
    {
        if (isImmune == false)
        {
            health = health - damage;

            // تأثير الضرر
            SimpleDamageFlash flash = GetComponent<SimpleDamageFlash>();
            if (flash != null)
                flash.FlashOnDamage();

            // باقي الكود...
        }
    }

    // دالة تأثير اللون الجديدة
    System.Collections.IEnumerator DamageColorEffect()
    {
        SpriteRenderer playerSR = GetComponent<SpriteRenderer>();
        if (playerSR != null)
        {
            Color originalColor = playerSR.color;
            playerSR.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            playerSR.color = originalColor;
        }
    }

    void SpriteFlicker()
    {
        if (flickerTime < flickerDuration)
        {
            flickerTime = flickerTime + Time.deltaTime;
        }
        else if (flickerTime >= flickerDuration)
        {
            sr.enabled = !(sr.enabled);
            flickerTime = 0;
        }
    }

}