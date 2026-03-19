using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float damage = 10f;//íeÇ™ó^Ç¶ÇÈÉ_ÉÅÅ[ÉWó 
	public float lifetime = 5f;//íeÇÃê∂ë∂éûä‘
	public float speed =10f;
	public GameObject hitEffectPrefab;

	public void Initialize(float bulletSpeed, float bulletDamage)
	{
		speed = bulletSpeed;
		damage = bulletDamage;
		Destroy(gameObject, lifetime);
		
	}

	void Update()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if(hitEffectPrefab!=null)
		{
			ContactPoint contact = collision.contacts[0];
			GameObject effec = Instantiate(
				hitEffectPrefab,
				contact.point,
				Quaternion.LookRotation(contact.normal)
				);
			Destroy(effec, 2f);
		}

			EnemyHP enemy = collision.gameObject.GetComponent<EnemyHP>();
		if(enemy != null)
		{
			enemy.TakeDamage(damage);
		}

		Destroy(gameObject);
	}
}