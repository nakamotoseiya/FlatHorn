using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public float damage = 1f;

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("Trigger Hit: " + other.name);

		if(other.CompareTag("Player"))
		{
			PlayerHP hp = other.GetComponent<PlayerHP>();
			if(hp != null)
			{
				hp.TakeDamage(damage);
			}
		}
	}
}
