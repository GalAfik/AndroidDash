using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
	public class GameManager : MonoBehaviour, IObserver
	{
		private GameObject Player;
		private GameObject EnemyManager;
		private int Score = 0; // How many points have been earned

		void Start()
		{
			// Get the player and bubbles manager objects
			Player = GameObject.FindWithTag("Player");
			EnemyManager = GameObject.FindWithTag("EnemyManager");
		}

		// Update is called once per frame
		void Update()
		{
			// If the player has no more health
			if (Player.GetComponent<Player>().Idle.GetHealth() <= 0)
			{
				// Stop all bubbles from chasing the player
				EnemyManager.GetComponent<EnemyManager>().StopAllEnemies();

				// TODO : end the game
			}
		}

		public void OnNotify(GameEvent e, int value)
		{
			switch (e)
			{
				case GameEvent.PLAYER_ATTACK:
				{
					print("GameManager::OnNotify.PLAYER_ATTACK");
					Score += value;
						print("GameManager::Score = " + Score);
					break;
				}
				case GameEvent.PLAYER_DASH:
				{
					print("GameManager::OnNotify.PLAYER_DASH");
					break;
				}
				case GameEvent.PLAYER_TAKE_DAMAGE:
				{
					print("GameManager::OnNotify.PLAYER_TAKE_DAMAGE");
					break;
				}
				case GameEvent.PLAYER_DEATH:
				{
					print("GameManager::OnNotify.PLAYER_DEATH");
					break;
				}
				default: break;
			}
		}
	}
}
