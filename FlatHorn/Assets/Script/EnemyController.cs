using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

[System.Serializable]
public class GroundRow
{
	public string rowName;         // 列の名前（Inspector用）
	public GameObject[] grounds;  // その列の地面オブジェクト
}

public class EnemyController : MonoBehaviour
{
	[Header("ステータス")]
	public float maxHP = 100f;
	private float currentHP;

	[Header("移動")]
	public float moveSpeed = 3.5f;
	public float rotationSpeed = 5f;
	public float attackDistance = 2.0f;

	[Header("攻撃")]
	public float attackCooldown = 1.2f;
	private float attackTimer = 0f;
	public float attackDurationFallback = 1.0f;
	public float attackDamage = 10f;

	[Header("Attack1")]
	public GameObject slashProjectilePrefab;    // 斬撃プレハブ
	public Transform slashFirePoint;          // 発射位置
	public float slashFireDelay = 0.3f;      //開始から発射までの秒数
	public float slashSpeed = 10f;
	public float slashDamage = 15f;           // 斬撃のダメージ
	public bool slashHoming = true;            // ホーミングするか
	public float slashHomingStrength = 4f;     // ホーミングの強さ
	public float slashHomingDuration = 1.5f;   // ホーミング持続秒数
	public float slashLifetime = 2f;          // 斬撃の生存秒数
	public AudioSource attack1Sound;



	[Header("Attack2 地面消去")]
    public GroundRow[] groundRows;
    public float groundDisappearDuration = 3f;
	public AudioSource attack2Sound;

	[Header("UI表示")]
	public TextMeshProUGUI Text;

	[Header("Attack3 落下攻撃")]
	public int attack3FallCount = 5;
	public float attack3SpawnHeight = 8f;
	public float attack3Radius = 3f;
	public float attack3Damage = 15f;
	public float attack3FallSpeed = 12f;
	public float attack3SpawnInterval = 0.2f;
	public AudioSource attack3Sound;


	private float verticalVelocity;
	public float gravity = -20f;

	private Transform player;
	private Animator animator;
	private CharacterController characterController;
	private FallingAttackObject fallingAttackObject;


	private bool isDead = false;
	private bool isAttacking = false;
	private bool isHit = false;

	void Awake()
	{
		animator = GetComponent<Animator>();
		characterController = GetComponent<CharacterController>();
		fallingAttackObject = GetComponent<FallingAttackObject>();


		if(animator == null || characterController == null)
		{
			enabled = false;
		}
	}

	void Start()
	{
		currentHP = maxHP;

		GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
		if(playerObj != null)
		{
			player = playerObj.transform;
		}
		if(attack1Sound != null)
		{
			attack1Sound.PlayOneShot(attack1Sound.clip);
			attack1Sound.Stop();
		}
		if(attack2Sound != null)
		{
			attack2Sound.PlayOneShot(attack2Sound.clip);
			attack2Sound.Stop();
		}
		if(attack3Sound != null)
		{
			attack3Sound.PlayOneShot(attack3Sound.clip);
			attack3Sound.Stop();
		}



		verticalVelocity = 0f;
		characterController.Move(Vector3.up * 0.2f);
	}

	void Update()
	{
		if(isDead || player == null)
			return;

		// 重力は常に適用
		ApplyGravity();

		// ヒット中は何もしない
		if(isHit)
			return;

		if(attackTimer > 0f)
			attackTimer -= Time.deltaTime;

		float distance = Vector3.Distance(transform.position, player.position);

		// 攻撃クールダウン更新
		//if(attackTimer > 0f)
		//{
		//	attackTimer -= Time.deltaTime;
		//}

		// 攻撃中は移動しない
		if(isAttacking)
		{
			UpdateAnimator(0f);
			return;
		}

		// 距離に応じて行動を決定
		if(distance > attackDistance)
		{
			Move();
			UpdateAnimator(distance);
		}
		else
		{
			// 攻撃距離内 & クールダウン終了
			if(attackTimer <= 0f)
			{
				Attack();
			}
			UpdateAnimator(0f);
		}
	}

	// 重力適用
	void ApplyGravity()
	{
		if(characterController.isGrounded)
		{
			if(verticalVelocity < 0)
				verticalVelocity = -2f;
		}
		else
		{
			verticalVelocity += gravity * Time.deltaTime;
		}

		// 重力だけ適用
		characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
	}

	// 移動処理
	void Move()
	{
		if(isAttacking || isHit)
			return;
		

		// プレイヤーの方向を向く
		Vector3 lookDir = player.position - transform.position;
		lookDir.y = 0f;

		if(lookDir != Vector3.zero)
		{
			Quaternion targetRotation = Quaternion.LookRotation(lookDir);
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				targetRotation,
				rotationSpeed * Time.deltaTime
			);
		}

		// 前方に移動（重力は別で適用済み）
		Vector3 move = transform.forward * moveSpeed;
		characterController.Move(move * Time.deltaTime);
	}

	// 攻撃処理
	void Attack()
	{
		if(isAttacking || isHit || isDead)
			return;

		isAttacking = true;

		// プレイヤーの方向を向く
		Vector3 dir = player.position - transform.position;
		dir.y = 0f;
		if(dir != Vector3.zero)
			transform.rotation = Quaternion.LookRotation(dir);


		// 全てのトリガーをリセット
		animator.ResetTrigger("Attack1");
		animator.ResetTrigger("Attack2");
		animator.ResetTrigger("Attack3");

		// ランダムな攻撃を選択
		int index = Random.Range(1, 4);
		animator.SetTrigger("Attack" + index);
		

        //Attack1：斬撃飛ばし
       if (index==1)
        {
			attack1Sound.Play();
			StartCoroutine(FireSlashAfterDelay());
		}

        //  Attack2：地面消去
        if (index == 2)
		{
			attack2Sound.Play();
			StartCoroutine(DisappearGround());
		}
		//  Attack3：落下攻撃
		if(index == 3)
		{
			attack3Sound.Play();

			if(fallingAttackObject != null)
			{
				fallingAttackObject.SpawnFallingAttack(
					player,               // Transform
					attack3FallCount,     // int
					attack3SpawnHeight,   // float
					attack3Radius,        // float
					attack3Damage,        // float
					attack3FallSpeed,     // float
					attack3SpawnInterval  // float
				);
			}
			else
			{
				Debug.LogWarning("FallingAttackObject が未設定のため Attack3 をスキップします。");
			}



		}

		Invoke(nameof(EndAttack), attackDurationFallback);

	}

	// Attack1 アニメーションイベント用
	private IEnumerator FireSlashAfterDelay()
	{
		

		yield return new WaitForSeconds(slashFireDelay);

		
		

		Transform spawnPoint = slashFirePoint != null ? slashFirePoint : transform;
		Vector3 toPlayer = (player.position - spawnPoint.position).normalized;
		Vector3 flatDir = new Vector3(toPlayer.x, 0f, toPlayer.z).normalized;

		

		if(flatDir == Vector3.zero)
		{
			
			yield break;
		}

		GameObject proj = Instantiate(
			slashProjectilePrefab,
			spawnPoint.position,
			Quaternion.LookRotation(flatDir)
		);

		

		SlashProjectile3D slash = proj.GetComponent<SlashProjectile3D>();
		if(slash == null)
		{
			
			yield break;
		}

		slash.Init(
			player,
			flatDir,
			slashSpeed,
			slashDamage,
			slashHoming,
			slashHomingStrength,
			slashHomingDuration,
			slashLifetime
		);

		
	}

	//Attack2
	private IEnumerator DisappearGround()
	{
		if(groundRows == null || groundRows.Length == 0)
		{
			
			yield break;
		}

		// ランダムに1列選ぶ
		int randomIndex = Random.Range(0, groundRows.Length);
		GroundRow selectedRow = groundRows[randomIndex];

		// テキスト表示
		if(Text != null)
		{
			Text.text = "どこかの地面が壊れました";
			Text.gameObject.SetActive(true);
		}

		// 選んだ列を非表示
		foreach(var ground in selectedRow.grounds)
			if(ground != null)
				ground.SetActive(false);

		yield return new WaitForSeconds(groundDisappearDuration);

		// 復活
		foreach(var ground in selectedRow.grounds)
			if(ground != null)
				ground.SetActive(true);

		// テキスト非表示
		if(Text != null)
		{
			Text.gameObject.SetActive(false);
		}
	}

	// Attack3
	


	// 攻撃ヒット
	public void AttackHit()
	{
		if(player == null || isDead)
			return;

		float distance = Vector3.Distance(transform.position, player.position);

		if(distance <= attackDistance)
		{
			PlayerHP hp = player.GetComponent<PlayerHP>();
			if(hp != null)
			{
				hp.TakeDamage(attackDamage);
			}
		}
	}


	// Animator 更新
	void UpdateAnimator(float distanceToPlayer)
	{
		float speed = 0f;

		if(!isAttacking && !isHit && !isDead)
		{
			if(distanceToPlayer > attackDistance)
			{
				speed = moveSpeed;
			}
		}

		animator.SetFloat("Speed", speed);
	}

	// ダメージ処理
	public void TakeDamage(float damage)
	{
		if(isDead)
			return;

		currentHP -= damage;

		isHit = true;
		isAttacking = false; // 攻撃中断
		CancelInvoke(nameof(EndAttack));


		animator.ResetTrigger("Hit");
		animator.SetTrigger("Hit");

		

		if(currentHP <= 0)
		{
			Die();
		}
	}
	

	// 死亡処理
	void Die()
	{
		if(isDead)
			return;

		isDead = true;

		animator.SetTrigger("Die");


		// コライダーを無効化
		if(characterController != null)
			characterController.enabled = false;



		Destroy(gameObject, 3f);

	}


	// 攻撃終了時に呼ばれる（Animationイベントから）
	public void EndAttack()
	{
		
		isAttacking = false;
		attackTimer = attackCooldown;
	}

	// ヒット終了時に呼ばれる（Animationイベントから）
	public void EndHit()
	{
		
		isHit = false;
	}

	// デバッグ用
	void OnDrawGizmosSelected()
	{
		// 攻撃範囲を可視化
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackDistance);
	}
}