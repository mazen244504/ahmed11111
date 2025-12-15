using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.03f;

    private TMP_Text textMesh;
    private string fullText;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
        fullText = textMesh.text;
    }

    private void OnEnable()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textMesh.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            textMesh.text += fullText[i];
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}