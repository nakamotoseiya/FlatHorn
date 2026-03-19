using UnityEngine;

public class LaserRotator : MonoBehaviour
{
	public float rotationSpeed = 90f;
	public float damage=1;

	void Update()
	{
		transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
	}

	
}