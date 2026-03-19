using UnityEngine;
using System.Collections;

public class FallingAttackObject : MonoBehaviour
{
	[Header("落下オブジェクトのPrefab")]
	public GameObject fallingObjectPrefab;

	public void SpawnFallingAttack(
		Transform playerTransform,
		int count = 5,
		float height = 8f,
		float radius = 3f,
		float dmg = 15f,
		float speed = 12f,
		float interval = 0.2f)
	{
		StartCoroutine(SpawnRoutine(playerTransform, count, height, radius, dmg, speed, interval));
	}

	// 球を順番に生成するコルーチン
	private IEnumerator SpawnRoutine(
		Transform playerTransform,
		int count, float height, float radius,
		float dmg, float speed, float interval)
	{
		{
			for(int i = 0; i < count; i++)
			{
				if(playerTransform == null)
					yield break;


				// プレイヤー周囲のランダム位置を決める
				Vector2 circle = Random.insideUnitCircle * radius;
				Vector3 spawnPos = playerTransform.position
								 + new Vector3(circle.x, height, circle.y);

				// Prefabから生成、なければプリミティブで代替
				GameObject ball;
				if(fallingObjectPrefab != null)
				{
					ball = Instantiate(fallingObjectPrefab, spawnPos, Quaternion.identity);
				}
				else
				{
					ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
					ball.transform.position = spawnPos;
					ball.transform.localScale = Vector3.one * 0.5f;
					Renderer rend = ball.GetComponent<Renderer>();
					if(rend != null)
						rend.material.color = new Color(1f, 0.2f, 0.2f);
				}

				ball.name = "FallingBall";
				ball.GetComponent<Collider>().isTrigger = true;

				// 落下・当たり判定は FallingBall に任せる
				FallingBall fallingBall = ball.GetComponent<FallingBall>();
				if(fallingBall == null)
					fallingBall = ball.AddComponent<FallingBall>();

				fallingBall.fallSpeed = speed;
				fallingBall.damage = dmg;



				yield return new WaitForSeconds(interval);
			}
		}
	}
}
