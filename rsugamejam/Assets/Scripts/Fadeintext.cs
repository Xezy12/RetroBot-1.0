using UnityEngine;
using TMPro;

public class Fadeintext : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private float fadeInDuration = 2.0f; // Duration of the fade-in effect

    void Start()
    {
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
        }

        // Set initial alpha to 0 to make the text transparent
        Color textColor = textMeshPro.color;
        textColor.a = 0f;
        textMeshPro.color = textColor;

        // Start the fade-in coroutine
        StartCoroutine(FadeInText());
    }

    private System.Collections.IEnumerator FadeInText()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            // Calculate the alpha value based on the elapsed time
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);

            // Set the text color with the new alpha value
            Color textColor = textMeshPro.color;
            textColor.a = alpha;
            textMeshPro.color = textColor;

            // Update elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the text is fully opaque at the end
        Color finalTextColor = textMeshPro.color;
        finalTextColor.a = 1f;
        textMeshPro.color = finalTextColor;
    }
}
