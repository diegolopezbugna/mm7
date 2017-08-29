using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class UnityExtensions
{
    public static IEnumerator Fade(this CanvasGroup canvasGroup, float from, float to, float duration, Action onFinished = null)
    {
        float elaspedTime = 0f;
        while (elaspedTime <= duration) {
            elaspedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elaspedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
        if (onFinished != null)
            onFinished();
    }
}

