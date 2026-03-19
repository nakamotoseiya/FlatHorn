using UnityEngine;

public class FallingBall : MonoBehaviour
{
	public float fallSpeed = 12f;
	public float damage = 15f;

	void Update()
	{
		// â∫Ç…óéâ∫
		transform.position += Vector3.down * fallSpeed * Time.deltaTime;

		// ínñ Ç…íÖÇ¢ÇΩÇÁè¡Ç¶ÇÈ
		if(transform.position.y <= 0f)
			Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			PlayerHP hp = other.GetComponent<PlayerHP>();
			if(hp != null)
				hp.TakeDamage(damage);

			Destroy(gameObject);
		}
	}
}