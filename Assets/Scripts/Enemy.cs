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
			public float MoveSpeed; // How fast this object moves toward the player
			public int ScoreValue; // How many points this bubble is worth
		}
		public ConfigurationData Conf = new ConfigurationData();
		protected bool IsTargetingPlayer = true;

		public void StopTargetingPlayer()
		{
			IsTargetingPlayer = false;
		}
	}
}
