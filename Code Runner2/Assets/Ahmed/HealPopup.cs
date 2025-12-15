using UnityEngine;
using TMPro;

public class HealPopup : MonoBehaviour
{
    public float floatSpeed = 30f;
    public float lifeTime = 1.2f;

    private TextMeshProUGUI text;
    private Color startColor;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        startColor = text.color;
    }

    public void SetText(string message, Color color)
    {
        text.text = message;
        text.color = color;
        startColor = color;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);
        text.color = Color.Lerp(startColor, Color.clear, Time.deltaTime * 2f);
    }
}
