using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
	public class Skeleton : Enemy
	{
		private GameObject Player; // The player this object should move towards

		void Start()
		{
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
	}
}