using UnityEngine;
using TMPro;

public class DeathUI : MonoBehaviour
{
    public static DeathUI Instance;

    [Header("UI References")]
    public GameObject deathPanel;      
    public TextMeshProUGUI killerText; 
    public TextMeshProUGUI countdownText;

    private void Awake()
    {
        Instance = this;
        deathPanel.SetActive(false);
    }

    public void ShowDeathCountdown(float time, string killerName)
    {
        deathPanel.SetActive(true);

        killerText.text = "You were killed by: " + killerName;

        StartCoroutine(Count(time));
    }

    private System.Collections.IEnumerator Count(float t)
    {
        while (t > 0)
        {
            countdownText.text = "Respawning in " + Mathf.Ceil(t).ToString();
            t -= Time.deltaTime;
            yield return null;
        }

        countdownText.text = "Respawning...";
        deathPanel.SetActive(false);
    }
}
