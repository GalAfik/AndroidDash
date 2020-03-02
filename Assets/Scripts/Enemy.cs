using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
	public interface IEnemyState
	{
		void Update(Enemy Enemy);
	}

	public class EnemySpawn : IEnemyState
	{
		public void Update(Enemy Enemy)
		{
			Enemy.StartCoroutine(Spawn(Enemy));
		}

		IEnumerator Spawn(Enemy Enemy)
		{
			yield return new WaitForSecondsRealtime(1);
			// Go to the moving state
			Enemy.CurrentState = Enemy.Moving;
		}
	}

	public class EnemyMoving : IEnemyState
	{
		public void Update(Enemy Enemy)
		{
			if (Enemy.IsTargetingPlayer)
			{
				// Move towards the player
				Enemy.transform.position = Vector3.MoveTowards(Enemy.transform.position, Enemy.Player.transform.position, Enemy.Conf.MoveSpeed * Time.deltaTime);
			}
		}
	}

	public class EnemyDead : IEnemyState
	{
		public void Update(Enemy Enemy)
		{
			throw new NotImplementedException();
		}
	}

	public class Enemy : MonoBehaviour
	{
		[Serializable]
		public class ConfigurationData
		{
			public bool FollowPlayer; // Should this object chase the player
			public float MoveSpeed; // How fast this object moves toward the player
			public int ScoreValue; // How many points this bubble is worth
		}
		public ConfigurationData Conf = new ConfigurationData();
		public bool IsTargetingPlayer;
		public GameObject Player; // The player this object should move towards

		// FSM States
		public IEnemyState CurrentState;
		public EnemySpawn Spawn = new EnemySpawn();
		public EnemyMoving Moving = new EnemyMoving();
		public EnemyDead Dead = new EnemyDead();

		private float spriteXScale;
		private float spriteYScale;

		void Start()
		{
			// Set the initial state
			CurrentState = Spawn;

			IsTargetingPlayer = Conf.FollowPlayer;
			// Grab a reference to the player object
			Player = GameObject.FindWithTag("Player");

			// Get the local scale for flipping the sprite later
			spriteXScale = transform.localScale.x;
			spriteYScale = transform.localScale.y;
		}

		// Update is called once per frame
		void Update()
		{
			// Delegate
			CurrentState.Update(this);

			// Turn to face the player
			if (Player.transform.position.x >= transform.position.x) transform.localScale = new Vector2(spriteXScale, spriteYScale);
			else transform.localScale = new Vector2(-spriteXScale, spriteYScale);
		}

		public void StopTargetingPlayer()
		{
			IsTargetingPlayer = false;
		}
	}
}
