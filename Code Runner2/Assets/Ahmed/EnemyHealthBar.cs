using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFill;

    private CanvasGroup canvasGroup;
    private int maxHealth;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f; // hidden at start
    }

    public void Setup(int max)
    {
        maxHealth = max;
        UpdateHealth(max);
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }

    public void UpdateHealth(int current)
    {
        if (maxHealth <= 0) return;
        healthFill.fillAmount = (float)current / maxHealth;
    }
}
