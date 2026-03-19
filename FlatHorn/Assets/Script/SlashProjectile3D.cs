using NUnit.Framework.Internal;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class SlashProjectile3D : MonoBehaviour
{

	private Transform _target;
	private Vector3 _dir;
	private float _speed;
	private float _damage;
	private bool _homing;
	private float _homingStrength;
	private float _homingDuration;


	private Rigidbody _rb;
	private float _homingTimer;
	private bool _initialized;
	

	void Awake()
	{
		_rb = GetComponent<Rigidbody>();
		_rb.useGravity = false;
		_rb.constraints = RigidbodyConstraints.FreezeRotation;

	}


	public void Init(
		Transform target,
		Vector3 direction,
		float speed,
		float damage,
		bool homing,
		float homingStrength,
		float homingDuration,
		float lifetime)
	{
		_target = target;
		_dir = direction.normalized;
		_speed = speed;
		_damage = damage;
		_homing = homing;
		_homingStrength = homingStrength;
		_homingDuration = homingDuration;
		_homingTimer = homingDuration;

		_rb.linearVelocity = _dir * _speed;
		_initialized = true;

		Destroy(gameObject, lifetime);
	}

	void FixedUpdate()
	{
		if(!_initialized)
			return;

		if(_homing && _target != null && _homingTimer > 0f)
		{
			_homingTimer -= Time.fixedDeltaTime;


			Vector3 toTarget = _target.position - transform.position;
			toTarget.y = 0f;

			if(toTarget.sqrMagnitude > 0.01f)
			{
				Vector3 desired = toTarget.normalized;

				_dir = Vector3.RotateTowards(
					_dir,
					desired,
					_homingStrength * Mathf.Deg2Rad * Time.fixedDeltaTime * 60f,
					0f
				);
			}
		}

		_rb.linearVelocity = _dir * _speed;


		if(_dir != Vector3.zero)
			transform.rotation = Quaternion.LookRotation(_dir);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			PlayerHP hp = other.GetComponent<PlayerHP>();
			if(hp != null)
				hp.TakeDamage(_damage);

			Destroy(gameObject);
		}
	}
}