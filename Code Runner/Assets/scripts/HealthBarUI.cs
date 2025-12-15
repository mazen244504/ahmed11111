using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("Heart Images")]
    public Image[] heartImages; // 3 صور للقلوب
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Player Reference")]
    public PlayerStats playerStats;

    void Start()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();

        UpdateHearts();
    }

    void Update()
    {
        UpdateHearts();
    }

    void UpdateHearts()
    {
        if (playerStats == null) return;

        for (int i = 0; i < heartImages.Length; i++)
        {
            // قلب مملوء إذا الصحة أكبر من الفهرس الحالي
            if (i < playerStats.health)
            {
                heartImages[i].sprite = fullHeart;
                heartImages[i].enabled = true;
            }
            // قلب فارغ
            else
            {
                heartImages[i].sprite = emptyHeart;
                heartImages[i].enabled = true;
            }

            // إخفاء القلوب الزائدة
            if (i >= playerStats.health)
            {
                heartImages[i].enabled = false;
            }
        }
    }
}