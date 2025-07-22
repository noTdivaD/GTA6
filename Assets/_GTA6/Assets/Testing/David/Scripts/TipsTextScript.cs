using TMPro;
using UnityEngine;

public class TipsTextScript: MonoBehaviour
{
    [Header("Tips Settings")]
    [TextArea(2, 5)]
    [SerializeField] public string[] tips;
    [SerializeField] public float interval = 5f;
    [SerializeField] public float fadeDuration = 1f;

    [Header("Components")]
    [SerializeField] private TMP_Text textMesh;
    [SerializeField] private int currentTipIndex = 0;
    [SerializeField] private float timer;
    private bool isFading = false;

    void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        if (tips.Length > 0)
        {
            textMesh.text = tips[0];
        }
    }

    void Update()
    {
        if (tips.Length == 0 || isFading) return;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            StartCoroutine(FadeToNextTip());
        }
    }

    System.Collections.IEnumerator FadeToNextTip()
    {
        isFading = true;

        // Fade out
        float t = 0f;
        Color originalColor = textMesh.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Swap text
        currentTipIndex = Random.Range(0, tips.Length);
        textMesh.text = tips[currentTipIndex];

        // Fade in
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        isFading = false;
    }
}