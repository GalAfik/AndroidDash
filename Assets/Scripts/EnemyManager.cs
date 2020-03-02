using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
	public class EnemyManager : MonoBehaviour
	{
		[Serializable]
		public struct SpawnChance
		{
			public GameObject EnemyPrefab; // The prefab to be spawned
			public int SpawnCount; // The chance this enemy will be spawned
		}
		[Serializable]
		public class ConfigurationData
		{
			public SpawnChance[] EnemySpawns; // The prefabs to be spawned and percentage each will be spawned at
			public int MaximumEnemyCapacity; // The maximum number of enemies that can active at the same time
			public int InitialEnemyCapacity; // The starting number of enemies that can active at the same time
			public float EnemyCapacityIncreaseRate; // How many seconds between each enemy capacity increase
		}
		public ConfigurationData Conf = new ConfigurationData();
		private List<GameObject> Enemies = new List<GameObject>(); // Enemies currently active
		private List<GameObject> EnemiesByCount = new List<GameObject>();
		private int EnemyCapacity;
		private bool SpawnEnemies = true;

		void Start()
		{
			// Set the initial enemy capacity
			EnemyCapacity = Conf.InitialEnemyCapacity;

			// Set up a list of spawn prefabs to choose from, by chance of spawn
			foreach (SpawnChance enemySpawnChance in Conf.EnemySpawns)
			{
				// Add an instance of the enemy type for each count of spawn chance
				for (int i = 0; i < enemySpawnChance.SpawnCount; i++)
				{
					EnemiesByCount.Add(enemySpawnChance.EnemyPrefab);
				}
			}
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
				int prefabIndex = UnityEngine.Random.Range(0, EnemiesByCount.Count);
				GameObject enemy = Instantiate(EnemiesByCount[prefabIndex], spawnPosition, Quaternion.identity, transform);
				Enemies.Add(enemy);
			}

			// Remove all destroyed enemies from the list
			Enemies.RemoveAll(enemies => enemies == null);

			// Start increasing the enemy capacity
			if (SpawnEnemies && EnemyCapacity < Conf.MaximumEnemyCapacity) StartCoroutine(IncreaseBubbleCapacity());
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
			// Stop spawning new enemies
			SpawnEnemies = false;

			// Stop all enemies from following the player
			foreach (GameObject enemy in Enemies)
			{
				if (enemy) enemy.GetComponent<Enemy>().StopTargetingPlayer();
			}
		}
	}
}