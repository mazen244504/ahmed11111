using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HintTrigger : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform hintPanel;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI dialogueText;
    public Button okButton;

    [Header("Text Messages")]
    [TextArea] public string hintMessage;
    [TextArea] public string rewardMessage;

    [Header("Animation Settings")]
    public float slideDuration = 0.35f;
    public float hiddenY = -300f;
    public float shownY = 80f;
    public float popScale = 1.05f;

    Coroutine currentRoutine;
    bool rewardShown;

    void Start()
    {
        // Panel OFF by default
        hintPanel.gameObject.SetActive(false);
        canvasGroup.alpha = 0f;
        hintPanel.anchoredPosition = new Vector2(0, hiddenY);
        hintPanel.localScale = Vector3.one;

        okButton.onClick.AddListener(OnOkPressed);
    }

    // 🔵 PLAYER ENTERS TRIGGER → PANEL ON
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        rewardShown = false;
        dialogueText.text = hintMessage;
        okButton.gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        hintPanel.gameObject.SetActive(true);
        currentRoutine = StartCoroutine(ShowHint());
    }

    // 🔵 PLAYER LEAVES TRIGGER → PANEL OFF
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(HideHint());
    }

    void OnOkPressed()
    {
        if (rewardShown) return;

        rewardShown = true;
        dialogueText.text = rewardMessage;
        okButton.gameObject.SetActive(false);

        // 🔫 unlock weapon logic can go here later
        Debug.Log("Weapon unlocked!");
    }

    IEnumerator ShowHint()
    {
        float t = 0f;
        hintPanel.localScale = Vector3.one * 0.95f;

        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float p = t / slideDuration;
            float eased = EaseOutCubic(p);

            float y = Mathf.Lerp(hiddenY, shownY, eased);
            hintPanel.anchoredPosition = new Vector2(0, y);

            canvasGroup.alpha = eased;
            hintPanel.localScale = Vector3.Lerp(Vector3.one * 0.95f, Vector3.one * popScale, eased);

            yield return null;
        }

        hintPanel.anchoredPosition = new Vector2(0, shownY);
        hintPanel.localScale = Vector3.one;
        canvasGroup.alpha = 1f;
    }

    IEnumerator HideHint()
    {
        float t = 0f;

        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float p = t / slideDuration;
            float eased = EaseInCubic(p);

            float y = Mathf.Lerp(shownY, hiddenY, eased);
            hintPanel.anchoredPosition = new Vector2(0, y);

            canvasGroup.alpha = 1f - eased;
            hintPanel.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.95f, eased);

            yield return null;
        }

        canvasGroup.alpha = 0f;
        hintPanel.localScale = Vector3.one;
        hintPanel.anchoredPosition = new Vector2(0, hiddenY);
        hintPanel.gameObject.SetActive(false);
    }

    float EaseOutCubic(float x)
    {
        return 1f - Mathf.Pow(1f - x, 3f);
    }

    float EaseInCubic(float x)
    {
        return x * x * x;
    }
}
