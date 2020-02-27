using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
	public class EnemyManager : MonoBehaviour
	{
		[Serializable]
		public class ConfigurationData
		{
			public GameObject[] EnemyPrefabs; // The prefabs to be spawned
			public int InitialEnemyCapacity; // The maximum number of enemies that can active at the same time
			public float EnemyCapacityIncreaseRate; // How many seconds between each enemy capacity increase
		}
		public ConfigurationData Conf = new ConfigurationData();
		private List<GameObject> Enemies = new List<GameObject>();
		private int EnemyCapacity;
		private bool SpawnEnemies = true;

		void Start()
		{
			// Set the initial enemy capacity
			EnemyCapacity = Conf.InitialEnemyCapacity;
		}

		// Update is called once per frame
		void Update()
		{
			// Spawn enemies every few seconds until capacity is reached
			if (Enemies.Count < EnemyCapacity)
			{
				Vector3 spawnPosition = new Vector3(
					UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x),
					UnityEngine.Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y),
					0
					);

				// Spawn a random enemy prefab from the array
				int prefabIndex = UnityEngine.Random.Range(0, Conf.EnemyPrefabs.Length);
				GameObject enemy = Instantiate(Conf.EnemyPrefabs[prefabIndex], spawnPosition, Quaternion.identity, transform);
				Enemies.Add(enemy);
			}

			// Remove all destroyed enemies from the list
			Enemies.RemoveAll(enemies => enemies == null);

			// Start increasing the enemy capacity
			if (SpawnEnemies) StartCoroutine(IncreaseBubbleCapacity());
		}

		IEnumerator IncreaseBubbleCapacity()
		{
			// Wait for a certain period
			SpawnEnemies = false;
			yield return new WaitForSecondsRealtime(Conf.EnemyCapacityIncreaseRate);

			// Increase the enemy capacity
			EnemyCapacity++;
			SpawnEnemies = true;
		}

		public void StopAllEnemies()
		{
			// Stop all enemies from following the player
			foreach (GameObject enemy in Enemies)
			{
				if (enemy) enemy.GetComponent<Enemy>().StopTargetingPlayer();
			}
		}
	}
}