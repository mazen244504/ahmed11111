using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject gameOverPanel; // Panel كامل
    public Text gameOverText;
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Settings")]
    public string gameOverMessage = "Game Over!";
    public float fadeDuration = 1f;

    private PlayerStats playerStats;
    private bool isGameOver = false;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        // إخفاء Panel بداية
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // إعداد الأزرار
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void Update()
    {
        // التحقق من Game Over
        if (!isGameOver && playerStats != null && playerStats.health <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;

        // إظهار Panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // تأثير Fade In
            CanvasGroup canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameOverPanel.AddComponent<CanvasGroup>();

            StartCoroutine(FadeIn(canvasGroup));
        }

        // تعطيل حركة اللاعب
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
            playerController.enabled = false;

        Debug.Log("Game Over!");
    }

    System.Collections.IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsed = 0f;
        canvasGroup.alpha = 0f;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public void RestartGame()
    {
        // إعادة تحميل المشهد الحالي
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        Debug.Log("Restarting game...");
    }

    public void GoToMainMenu()
    {
        // تحميل مشهد Main Menu
        SceneManager.LoadScene("MainMenu");

        Debug.Log("Going to main menu...");
    }
}