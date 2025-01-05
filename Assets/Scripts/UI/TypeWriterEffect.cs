using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent; 
    public float typingSpeed = 0.05f;
    

    private void Start()
    {
        textComponent.text = "";
    }

    public void StartTypewriter(string fullText)
    {
        textComponent.text = "";  
        StartCoroutine(TypeText(fullText));
    }

    private IEnumerator TypeText(string fullText)
    {
        textComponent.text = "";
        yield return null;
        foreach (char letter in fullText)
        {
            textComponent.text += letter; 
            yield return new WaitForSeconds(typingSpeed); 
        }
    }
}
