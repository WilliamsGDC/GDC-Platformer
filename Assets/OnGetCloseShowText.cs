using UnityEngine;
using System.Collections;
using TMPro;

public class Show3DTextOnProximity : MonoBehaviour
{
    public string playerTag = "Player";            // Tag for the player object
    public TextMeshPro text3DObject;               // Reference to TextMeshPro component
    public float displayDuration = 2f;             // How long the text stays visible (optional)
    public float fadeDuration = 1f;                // Duration of the fade-in

    private Coroutine fadeCoroutine;
    private Coroutine hideTextCoroutine;

    private void Start()
    {
        if (text3DObject != null)
        {
            SetTextAlpha(0);                       // Make text fully transparent at start
            text3DObject.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (text3DObject != null)
            {
                text3DObject.gameObject.SetActive(true);

                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeTextIn());

                if (displayDuration > 0)
                {
                    if (hideTextCoroutine != null)
                        StopCoroutine(hideTextCoroutine);
                    hideTextCoroutine = StartCoroutine(HideTextAfterDelay(displayDuration));
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && displayDuration <= 0)
        {
            if (text3DObject != null)
            {
                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeTextOut());
            }
        }
    }

    private IEnumerator FadeTextIn()
    {
        float elapsed = 0f;
        SetTextAlpha(0);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            SetTextAlpha(alpha);
            yield return null;
        }

        SetTextAlpha(1);
    }

    private IEnumerator FadeTextOut()
    {
        float elapsed = 0f;
        SetTextAlpha(1);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsed / fadeDuration);
            SetTextAlpha(alpha);
            yield return null;
        }

        SetTextAlpha(0);
        text3DObject.gameObject.SetActive(false);
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return FadeTextOut();
    }

    private void SetTextAlpha(float alpha)
    {
        Color color = text3DObject.color;
        color.a = alpha;
        text3DObject.color = color;
    }
}
