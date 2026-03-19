using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
	public GameObject potionPrefab;
	public int spawnCount = 2;   // 生成するポーション数
	public float rangeX = 20f;   // X方向の生成範囲
	public float rangeZ = 20f;   // Z方向の生成範囲
	public float spawnY = 1f;    // 地面からの高さ

	void Start()
	{
		if(hasSpawned)
			return; // 生成済みなら処理しない
		hasSpawned = true;

		for(int i = 0; i < spawnCount; i++)
		{
			float randomX = Random.Range(-rangeX, rangeX);
			float randomZ = Random.Range(-rangeZ, rangeZ);
			Vector3 spawnPos = new Vector3(randomX, spawnY, randomZ);
			Instantiate(potionPrefab, spawnPos, Quaternion.identity);
		}
	}

	private bool hasSpawned = false;
}
