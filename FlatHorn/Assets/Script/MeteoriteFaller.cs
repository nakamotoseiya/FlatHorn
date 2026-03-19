using UnityEngine;

/// <summary>
/// isiプレハブにアタッチする落下専用スクリプト
/// FallingAttackObjectのSpawnRoutineから呼ばれる
/// </summary>
public class MeteoriteFaller : MonoBehaviour
{
	private Vector3 startPos;
	private Vector3 endPos;
	private float fallTime;
	private float elapsed = 0f;
	public bool isFalling = false;
	private bool hasHit = false;
	private float damage;

	void Awake()
	{
		// Rigidbodyを無効化
		Rigidbody rb = GetComponent<Rigidbody>();
		if(rb != null)
		{
			rb.isKinematic = true;
			rb.useGravity = false;
		}

		// FallingMeteoriteAttackが残っていれば無効化
		FallingMeteoriteAttack fma = GetComponent<FallingMeteoriteAttack>();
		if(fma != null)
			fma.enabled = false;
	}

	public void StartFall(Vector3 from, Vector3 to, float duration, float dmg)
	{
		startPos = from;
		endPos = to;
		fallTime = duration;
		damage = dmg;
		elapsed = 0f;
		isFalling = true;
	}


	void Update()
	{
		if(!isFalling || hasHit)
			return;

		elapsed += Time.deltaTime;

		float t = elapsed / fallTime;

		transform.position = Vector3.Lerp(startPos, endPos, t);

		if(elapsed >= fallTime)
		{
			transform.position = endPos;
			isFalling = false;
			Destroy(gameObject, 1f);
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if(hasHit)
			return;

		// 隕石同士は無視
		if(other.GetComponent<MeteoriteFaller>() != null)
			return;
		// 敵は無視
		if(other.GetComponent<EnemyController>() != null)
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