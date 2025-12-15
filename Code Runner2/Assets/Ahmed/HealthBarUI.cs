using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Image fillImage;

    private int maxHealth;

    public void SetMaxHealth(int max)
    {
        maxHealth = max;
        fillImage.fillAmount = 1f;
    }

    public void SetHealth(int current)
    {
        fillImage.fillAmount = (float)current / maxHealth;
    }
}
