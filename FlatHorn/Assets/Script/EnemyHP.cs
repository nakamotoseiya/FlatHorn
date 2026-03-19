using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EnemyHP : MonoBehaviour
{ 
[Header("HP")]
	public float maxHP = 1000f;
	private float currentHP;

[Header("HPバー")]
	public Slider hpBarSlider;
	public AudioSource Enmeydamaged;

	[Header("フェードアウト")]
	public Image fadeImage;           // 黒いImageをアサイン
	public float fadeDuration = 1.5f; // フェード時間

	void Start()
	{
		currentHP = maxHP;
	
		UpdateHPBar();

		// フェード用Imageを透明に初期化
		if(fadeImage != null)
		{
			Color c = fadeImage.color;
			c.a = 0f;
			fadeImage.color = c;
		}
	}
void Update()
{
	// HPバーを常にカメラの方向に向ける
	if(hpBarSlider != null)
	{
		hpBarSlider.transform.LookAt(Camera.main.transform); // 180度回転させて正面を向かせる
	}

}
// ダメージを受ける処理
public void TakeDamage(float damage)
{
	currentHP -= damage;
	currentHP = Mathf.Clamp(currentHP, 0, maxHP);
		if(Enmeydamaged != null)
		{
			Enmeydamaged.PlayOneShot(Enmeydamaged.clip);
		}
		UpdateHPBar();
		

	if(currentHP <= 0)
	{
		Die();
	}
}
void UpdateHPBar()
{
	if(hpBarSlider  != null)
	{
			hpBarSlider.value = currentHP / maxHP;
	}
	

}

void Die()
	{
	
	Destroy(gameObject);
		StartCoroutine(FadeAndLoadScene());
		SceneManager.LoadScene("GameClear Scene");
	}

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

		
	}
}
