using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerPopUpManager : MonoBehaviour
{
    [Header("You Died")]
    [SerializeField] GameObject youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI youDiedPopUpText;
    [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;

    public void SendYouDiedPopUp()
    {
        youDiedPopUpGameObject.SetActive(true);
        youDiedPopUpBackgroundText.characterSpacing = 0;
        StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgroundText, 8f, 15f));
        StartCoroutine(FadeInPopupOverTime(youDiedPopUpCanvasGroup, 5));
        StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
    }

    private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
    {
        if (duration > 0)
        {
            text.characterSpacing = 0;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }
    }

    private IEnumerator FadeInPopupOverTime(CanvasGroup canvas, float duration)
    {
        if (duration > 0)
        {
            canvas.alpha = 0;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }

        canvas.alpha = 1;
        yield return null;
    }

    private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)
    {
        if (duration > 0)
        {

            while (delay > 0)
            {
                delay -= Time.deltaTime;
                yield return null;
            }
            canvas.alpha = 1;
            float timer = 0;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }

        canvas.alpha = 0;
        yield return null;
    }
}
