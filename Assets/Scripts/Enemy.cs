using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
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
		protected bool IsTargetingPlayer;
		protected GameObject Player; // The player this object should move towards

		void Awake()
		{
			IsTargetingPlayer = Conf.FollowPlayer;
			// Grab a reference to the player object
			Player = GameObject.FindWithTag("Player");
		}

		// Update is called once per frame
		void Update()
		{
			if (IsTargetingPlayer)
			{
				// Move towards the player
				transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Conf.MoveSpeed * Time.deltaTime);
			}
		}

		public void StopTargetingPlayer()
		{
			IsTargetingPlayer = false;
		}
	}
}
