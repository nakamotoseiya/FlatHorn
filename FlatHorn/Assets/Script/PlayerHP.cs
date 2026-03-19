using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
	[Header("HP")]
	public float maxHP = 100f;
	private float currentHP;

	[Header("HPバー")]
	public Slider hpBar;

	[Header("ダメージエフェクト")]
	public float flashDuration = 0.4f; // 赤くなる時間
	public AudioSource damaged;

	private Renderer[] renderers;
	private Color[] originalColors;
	private Coroutine flashCoroutine;

	[Header("フェードアウト")]
	public Image fadeImage;
	public float fadeDuration = 1.5f;

	void Start()
	{
		currentHP = maxHP;
		if(damaged != null)
		{
			damaged.PlayOneShot(damaged.clip);
			damaged.Stop();
		}
		// 子オブジェクト含む全Rendererを取得
		renderers = GetComponentsInChildren<Renderer>();

		// 元の色を保存
		originalColors = new Color[renderers.Length];
		for(int i = 0; i < renderers.Length; i++)
		{
			originalColors[i] = renderers[i].material.color;
		}

		// フェード用Imageを透明に初期化
		if(fadeImage != null)
		{
			Color c = fadeImage.color;
			c.a = 0f;
			fadeImage.color = c;
		}
	}

	public void TakeDamage(float damage)
	{
		currentHP -= damage;
		currentHP = Mathf.Max(currentHP, 0f);
		HpBar();
		Debug.Log($"Player HP: {currentHP}");

		// 赤くなる処理
		if(flashCoroutine != null)
			StopCoroutine(flashCoroutine);
		flashCoroutine = StartCoroutine(FlashRed());

		if(currentHP <= 0f)
		{
			Die();
			
		}
	}
	//ポーション回復
	void HpBar(bool playSound = true)
	{
		if(hpBar != null)
		{
			if(playSound && damaged != null)
				damaged.Play();
			hpBar.value = currentHP / maxHP;
		}
	}
	public void Heal(float amount)
	{
		currentHP += amount;
		currentHP = Mathf.Min(currentHP, maxHP);
		HpBar(false);
		Debug.Log($"Player HP: {currentHP}");
	}

	IEnumerator FlashRed()
	{
		// 全パーツを赤に
		foreach(var r in renderers)
			r.material.color = Color.red;

		float elapsed = 0f;
		while(elapsed < flashDuration)
		{
			elapsed += Time.deltaTime;
			float t = elapsed / flashDuration;

			// 赤 → 元の色にフェード
			for(int i = 0; i < renderers.Length; i++)
				renderers[i].material.color = Color.Lerp(Color.red, originalColors[i], t);

			yield return null;
		}

		// 完全に元の色に戻す
		for(int i = 0; i < renderers.Length; i++)
			renderers[i].material.color = originalColors[i];
	}
			
	private void Die()
	{
		Debug.Log("Player is Dead");
		this.enabled = false;
		StartCoroutine(FadeAndLoadScene());
		IEnumerator FadeAndLoadScene()
	{
		float elapsed = 0f;

		while(elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			if(fadeImage != null)
			{
				Color c = fadeImage.color;
				c.a = Mathf.Clamp01(elapsed / fadeDuration);
				fadeImage.color = c;
			}
			yield return null;
		}

		SceneManager.LoadScene("GameOver Scene");
	}
}
}
