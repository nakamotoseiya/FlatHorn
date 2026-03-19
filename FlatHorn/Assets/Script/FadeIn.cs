// FadeIn.cs（新規作成）
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
	[Header("フェード設定")]
	public Image fadeImage;
	public float fadeInDuration = 1.5f;

	void Start()
	{
		StartCoroutine(FadeInCoroutine());
	}

	IEnumerator FadeInCoroutine()
	{
		float elapsed = 0f;
		SetAlpha(1f); // 最初は真っ黒

		while(elapsed < fadeInDuration)
		{
			elapsed += Time.deltaTime;
			SetAlpha(1f - Mathf.Clamp01(elapsed / fadeInDuration));
			yield return null;
		}

		SetAlpha(0f); // 完全に透明
	}

	void SetAlpha(float alpha)
	{
		if(fadeImage == null)
			return;
		Color c = fadeImage.color;
		c.a = alpha;
		fadeImage.color = c;
	}
}