using UnityEngine;
using TMPro;

public class TeleportTrigger : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI pressQText;

    [Header("Teleport Destination")]
    public Transform destinationPoint;

    private bool playerInside = false;
    private Transform player;

    private void Start()
    {
        pressQText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.transform;

            pressQText.text = "Press Q to teleport to last question";
            pressQText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            pressQText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.Q))
        {
            pressQText.gameObject.SetActive(false);
            player.position = destinationPoint.position;
        }
    }
}
