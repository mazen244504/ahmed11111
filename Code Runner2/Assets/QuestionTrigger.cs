using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    public QuestionUI questionUI;
    public GameObject pressQText;

    private bool playerInside = false;

    private void Start()
    {
        pressQText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            pressQText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            pressQText.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.Q))
        {
            pressQText.SetActive(false);
            questionUI.OpenQuestion();
            playerInside = false; // prevent reopening
        }
    }
}
