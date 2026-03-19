using UnityEngine;

public class AttackPowerUp : MonoBehaviour
{
	public float duration = 10f;
	public AudioSource potionUpSound;


	private void OnTriggerEnter(Collider other)
	{

		PlayerController player = other.GetComponent<PlayerController>();

		if(player != null)
		{
			if(potionUpSound != null)
			{
				// 音だけ別のオブジェクトで鳴らす
				AudioSource.PlayClipAtPoint(potionUpSound.clip, transform.position);
			}

			player.ActivatePowerUp(duration);
			Destroy(gameObject);

		}
	}

}
