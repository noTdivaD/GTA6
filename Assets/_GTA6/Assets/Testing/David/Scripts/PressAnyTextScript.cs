using UnityEngine;
using TMPro;
using System.Collections;

public class PressAnyTextScript : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] public float fadeDuration = 1f;
    [SerializeField] private TMP_Text textComponent;

    void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        StartCoroutine(FadeLoop());
    }

    // Alternate between visible and invisible
    private IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return StartCoroutine(FadeTextToFullAlpha(fadeDuration, textComponent));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(FadeTextToZeroAlpha(fadeDuration, textComponent));
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Fade in
    public IEnumerator FadeTextToFullAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    // Fade out
    public IEnumerator FadeTextToZeroAlpha(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
