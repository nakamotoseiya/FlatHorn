using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
	[Header("移動設定")]
	public float moveSpeed = 5f;
	public float runSpeed = 10f;
	public float jumpForce = 7f;
	public float gravity = -9.81f;

	[Header("カメラ設定")]
	public Transform cameraTransform;
	public float mouseSensitivity = 2f;
	public float maxLookAngle = 80f;

	[Header("射撃設定")]
	public Transform gunBarrel;
	public GameObject bulletPrefab;
	public float bulletSpeed = 10f;
	public float fireRate = 0.1f;
	


	[Header("攻撃力設定")]
	public float baseDamage = 10f;

	private float damageMultiplier = 1f;

	[Header("UI表示")]
	public TextMeshProUGUI powerUpText;
	private float powerUpTimer;
	private bool isPowerUpActive = false;


	[Header("アニメーション")]
	public Animator animator;

	// プライベート変数
	private CharacterController controller;
	private Vector3 velocity;
	private bool isGrounded;
	private float cameraPitch = 0f;
	private float nextFireTime = 0f;

	//アニメーションパラメータ名
	private readonly int isMovingHash = Animator.StringToHash("IsMoving");
	private readonly int isRunningHash = Animator.StringToHash("IsRunning");
	private readonly int isGroundedHash = Animator.StringToHash("IsGrounded");
	//private readonly int jumpHash = Animator.StringToHash("Jump");
	private readonly int shootHash = Animator.StringToHash("Shoot");

	void Start()
	{
		controller = GetComponent<CharacterController>();

		// カーソルをロック
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible=false;

	}

	void Update()
	{
		HandleMovement();
		HandleCamera();
		HandleJump();
		HandleShooting();
		HandleCursorLock();
		FootStep();


		if(Input.GetKeyDown(KeyCode.P))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			SceneManager.LoadScene("Title Scene"); 

		}
	}

	void HandleMovement()
	{
		
		isGrounded = controller.isGrounded;

		if(isGrounded && velocity.y < 0)
			velocity.y = -2f;

		float horizontal = 0f;
		float vertical = 0f;

		if(Input.GetKey(KeyCode.W))
			vertical += 1f;
		if(Input.GetKey(KeyCode.S))
			vertical -= 1f;
		if(Input.GetKey(KeyCode.D))
			horizontal += 1f;
		if(Input.GetKey(KeyCode.A))
			horizontal -= 1f;

		Vector3 move =
			transform.right * horizontal +
			transform.forward * vertical;

		bool isRunning = Input.GetKey(KeyCode.LeftShift);
		float speed = isRunning ? runSpeed : moveSpeed; 

		Vector3 finalMove = move * speed;
		finalMove.y = velocity.y;

		controller.Move(finalMove * Time.deltaTime);

		velocity.y += gravity * Time.deltaTime;

		// アニメーション
		if(animator != null)
		{
			bool isMoving = (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f);

			animator.SetBool(isMovingHash, isMoving);
			animator.SetBool(isRunningHash, isMoving && isRunning);
			animator.SetBool(isGroundedHash, isGrounded);
		}
	}


	void HandleCamera()
	{
		
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

		
		transform.Rotate(Vector3.up * mouseX);

		
		cameraPitch -= mouseY;
		cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);
		cameraTransform.localEulerAngles = Vector3.right * cameraPitch;
	}

	void HandleJump()
	{
		if(Input.GetKeyDown(KeyCode.Space)&&isGrounded)
		{
			velocity.y=Mathf.Sqrt(jumpForce * -2f * gravity);
			if(animator!=null)
			{
				//animator.SetTrigger(jumpHash);
			}
		}
		
		velocity.y+=gravity * Time.deltaTime;
		
	}

	void HandleShooting()
	{
		if(Input.GetButton("Fire1") && Time.time >= nextFireTime)
		{
			Shoot();
			nextFireTime = Time.time + fireRate;
		}
	}

	void Shoot()
	{
		if(animator != null)
		{
			animator.SetTrigger(shootHash);
		}

		if(bulletPrefab == null || gunBarrel == null)
			return;

		Vector3 dir = cameraTransform.forward;

		GameObject bullet = Instantiate(
			bulletPrefab,
			gunBarrel.position,
			Quaternion.LookRotation(dir)
		);

		Bullet bulletScript = bullet.GetComponent<Bullet>();
		if(bulletScript != null)
		{
			bulletScript.Initialize(bulletSpeed, CurrentDamage);
		}


	}

	void FootStep(){ }

	//攻撃力
	public float CurrentDamage
	{
		get
		{
			return baseDamage * damageMultiplier;
		}
	}
	public void ActivatePowerUp(float duration)
	{
		StopCoroutine("PowerUpRoutine");
		StartCoroutine(PowerUpRoutine(duration));
	}

	public IEnumerator PowerUpRoutine(float duration)
	{
		damageMultiplier = 10f;
		powerUpTimer = duration;
		isPowerUpActive = true;

		powerUpText.gameObject.SetActive(true);

		while(powerUpTimer>0)
		{
			powerUpTimer -= Time.deltaTime;
			if(powerUpText != null)
				powerUpText.text = "攻撃力強化中 "
					+ powerUpTimer.ToString("F1") + "秒";
			yield return null;
		}

		damageMultiplier = 1f;
		isPowerUpActive = false;

		powerUpText.text = "強化終了";
		yield return new WaitForSeconds(1f);

		if(powerUpText!=null)
		powerUpText.gameObject.SetActive(false);
	}

	
	

	void HandleCursorLock()
	{
		// ESCキーでカーソルのロック解除/再ロック
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			if(Cursor.lockState == CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}

		}
	}


}


