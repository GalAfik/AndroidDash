using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
	public class GameManager : MonoBehaviour, IObserver
	{
		[Serializable]
		public class ConfigurationData
		{
			public GameObject PlayerObject;
			public GameObject EnemyManagerObject;
			public GameObject UICanvasObject;
		}
		[Serializable]
		private class StateData
		{
			public Player Player;
			public EnemyManager EnemyManager;
			public GameUserInterface UICanvas;
			public int Score = 0; // How many points have been earned
		}
		public ConfigurationData Conf = new ConfigurationData();
		private StateData State = new StateData();


		void Start()
		{
			// Get private class references for all public objects
			State.Player = Conf.PlayerObject.GetComponent<Player>();
			State.EnemyManager = Conf.EnemyManagerObject.GetComponent<EnemyManager>();
			State.UICanvas = Conf.UICanvasObject.GetComponent<GameUserInterface>();
		}

		// Update is called once per frame
		void Update()
		{
			// If the player has no more health
			if (State.Player.CurrentState == State.Player.Dead)
			{
				// Stop all bubbles from chasing the player
				State.EnemyManager.StopAllEnemies();

				// TODO : end the game
				
			}
		}

		// Runs when the GUI is updated
		private void OnGUI()
		{
			// Update the score UI text
			State.UICanvas.UpdateScore(State.Score);

			// Update the player health UI
			State.UICanvas.UpdateHearts(State.Player.Idle.GetHealth());
		}

		public void OnNotify(GameEvent e, int value)
		{
			switch (e)
			{
				case GameEvent.PLAYER_ATTACK:
				{
					print("GameManager::OnNotify.PLAYER_ATTACK");
					// Update the score
					State.Score += value;
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
