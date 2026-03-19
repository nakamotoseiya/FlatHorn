using UnityEngine;

public class Potion : MonoBehaviour
{
	public float healAmount = 0f;// 回復量
	public AudioSource potionSound;// 効果音

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			PlayerHP player = other.GetComponent<PlayerHP>();
			if(player != null)
			{
				player.Heal(healAmount);

				if(potionSound != null)
				{
					// 音だけ別のオブジェクトで鳴らす
					AudioSource.PlayClipAtPoint(potionSound.clip, transform.position);
				}

				Destroy(gameObject); // すぐ消える
			}
		}
	}
}

