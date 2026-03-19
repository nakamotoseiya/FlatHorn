using UnityEngine;

public class FallingMeteoriteAttack : MonoBehaviour
{
	private float fallSpeed;
	private float damage;
	private bool hasHit = false;

	public void Init(float speed, float dmg)
	{
		fallSpeed = speed;
		damage = dmg;
	}

	void Update()
	{
		if(hasHit)
			return;

		// 1回だけ落下（重複削除）
		transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter(Collider other)
	{
		if(hasHit)
			return;

		// 敵は無視
		if(other.GetComponent<EnemyController>() != null)
			return;
		// 隕石同士は無視
		if(other.GetComponent<FallingMeteoriteAttack>() != null)
			return;

		// プレイヤーにダメージ
		PlayerHP hp = other.GetComponent<PlayerHP>();
		if(hp != null)
		{
			hasHit = true;
			hp.TakeDamage(damage);
			Destroy(gameObject);
			return;
		}

		// それ以外（地面など）は消える
		hasHit = true;
		Destroy(gameObject);
	}
}