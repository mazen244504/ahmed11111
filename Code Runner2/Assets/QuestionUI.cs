using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class QuestionUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public Button[] answerButtons;
    public TextMeshProUGUI resultText;

    [Header("Correct Answer")]
    public int correctAnswerIndex;

    [Header("Respawn Points")]
    public Transform rightSpawn;
    public Transform wrongSpawn;

    [Header("Player")]
    public Transform player;

    private bool answered;

    private void Start()
    {
        panel.SetActive(false);
        resultText.gameObject.SetActive(false);
    }

    public void OpenQuestion()
    {
        panel.SetActive(true);
        Time.timeScale = 0f; // pause game
        answered = false;

        resultText.gameObject.SetActive(false);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => Answer(index));
        }
    }

    void Answer(int index)
    {
        if (answered) return;
        answered = true;

        if (index == correctAnswerIndex)
            StartCoroutine(RightAnswerRoutine());
        else
            StartCoroutine(WrongAnswerRoutine());
    }

    // ✅ RIGHT ANSWER
    IEnumerator RightAnswerRoutine()
    {
        resultText.text = "✅ CORRECT!";
        resultText.color = Color.green;
        resultText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        panel.SetActive(false);

        player.position = rightSpawn.position;
    }

    // ❌ WRONG ANSWER (NEW & FIXED)
    IEnumerator WrongAnswerRoutine()
    {
        resultText.text = "❌ WRONG! You will restart...";
        resultText.color = Color.red;
        resultText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        panel.SetActive(false);

        player.position = wrongSpawn.position;
    }
}
