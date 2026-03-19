using UnityEngine;
using System.Collections;
public class  FadeOut: MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public float fadeDuration = 1.0f;

	public IEnumerator FadeOutCoroutine()
	{
		float elapsed = 0f;
		canvasGroup.alpha = 1f;

		while(elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			canvasGroup.alpha = 1f - (elapsed / fadeDuration);
			yield return null;
		}

		canvasGroup.alpha = 0f;
	}

	void Start()
	{
		StartCoroutine(FadeOutCoroutine());
	}
}
