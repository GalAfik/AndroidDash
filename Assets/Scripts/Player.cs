using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
	public interface IPlayerState
	{
		void Update(Player Player);
		void OnTriggerEnter2D(Player Player, Collider2D collision);
	}

	[SelectionBase]
	public class PlayerIdle : IPlayerState
	{
		private int Health;

		public void SetHealth(int health)
		{
			Health = health;
		}

		public int GetHealth()
		{
			return Health;
		}

		// Draw a line between start and end
		private void DrawLine(Player Player, Vector3 start, Vector3 end, Color? color = null, float width = 0.1f)
		{
			GameObject myLine = new GameObject();
			myLine.transform.position = start;
			myLine.AddComponent<LineRenderer>();
			LineRenderer lr = myLine.GetComponent<LineRenderer>();
			lr.material = new Material(Player.Conf.LineShader);
			lr.startColor = color ?? Color.black;
			lr.endColor = color ?? Color.black;
			lr.startWidth = width;
			lr.endWidth = width;
			lr.SetPosition(0, start);
			lr.SetPosition(1, end);
			GameObject.Destroy(myLine);
		}

		public void Update(Player Player)
		{
			Vector3 destination = Vector3.zero;
			// Only take in input when the player has reached their destination and are no longer dashing
			if (Input.touchSupported && Input.touchCount > 0)
			{
				// Get the touch input and set the destination to it
				Touch touch = Input.GetTouch(0);
				switch (touch.phase)
				{
					case TouchPhase.Began :
						{
							// Draw a line between the player and the mouse/finger when holding down
							DrawLine(Player, Player.transform.position, touch.position);
							break;
						}
					case TouchPhase.Ended: // Set final destination
						{
							destination = touch.position;
							Player.CurrentState = Player.Dashing;
							break;
						}
					default: break;
				}
			}

			// Draw a line between the player and the mouse/finger when holding down
			if (Input.GetMouseButton(0))
			{
				DrawLine(Player, Player.transform.position, Input.mousePosition);
			}
			// Set final destination
			if (Input.GetMouseButtonUp(0))
			{
				// Get the mouse position in the world and set the destination to it
				destination = MonoBehaviour.FindObjectOfType<Camera>().ScreenToWorldPoint(Input.mousePosition);
				Player.CurrentState = Player.Dashing;
			}

			// Ignore the z position of the mouse
			destination = Vector3.Scale(destination, new Vector3(1, 1, 0));
			Player.Dashing.SetDestination(destination);
		}

		public void OnTriggerEnter2D(Player Player, Collider2D collision)
		{
			// Destroy the other object
			MonoBehaviour.Destroy(collision.gameObject);

			// Decrease health
			Health--;
		}
	}

	public class PlayerDashing : IPlayerState
	{
		private Vector3 Destination;

		public void SetDestination(Vector3 destination)
		{
			Destination = destination;
		}

		public void Update(Player Player)
		{
			if (Player.transform.position != Destination)
			{
				// Move to the destination position
				Player.transform.position = Vector3.MoveTowards(Player.transform.position, Destination, Player.Conf.MoveSpeed * Time.deltaTime);
			}
			else
			{
				// Reset back to the idle state
				Player.CurrentState = Player.Idle;
			}

			// Flip the sprite to look in the direction the player is dashing
			Vector2 movementVector = Destination - Player.transform.position;
			if (movementVector.x > 0) Player.transform.localScale = new Vector2(1, 1);
			else if (movementVector.x < 0) Player.transform.localScale = new Vector2(-1, 1);
		}

		public void OnTriggerEnter2D(Player Player, Collider2D collision)
		{
			// Destroy any enemies the player touches while dashing
			if (collision.gameObject.CompareTag("Enemy"))
			{
				MonoBehaviour.Destroy(collision.gameObject);
				// Notify the game manager of a player attack
				int scoreValue = collision.gameObject.GetComponent<Enemy>().Conf.ScoreValue;
				Player.Notify(GameEvent.PLAYER_ATTACK, scoreValue);
			}
		}
	}

	public class PlayerDead : IPlayerState
	{
		public void Update(Player Player) {}
		public void OnTriggerEnter2D(Player Player, Collider2D collision) {}
	}

	public class Player : MonoBehaviour
	{
		[Serializable]
		public class ConfigurationData
		{
			[Header("Movement Vars")]
			public float MoveSpeed;

			[Header("Observer Objects")]
			public GameObject[] ObserverObjects;

			[Header("Other")]
			public int MaxHealth;
			public Shader LineShader; // The shader used to draw a line when the player is ready to dash
		}
		public ConfigurationData Conf = new ConfigurationData();

		// FSM States
		public IPlayerState CurrentState;
		public PlayerIdle Idle = new PlayerIdle();
		public PlayerDashing Dashing = new PlayerDashing();

		// List of observers to this object
		private List<IObserver> Observers = new List<IObserver>();

		void Start()
		{
			// Add all observers
			foreach (GameObject observer in Conf.ObserverObjects)
			{
				Observers.Add(observer.GetComponent<IObserver>());
			}

			// Set the initial sttate
			CurrentState = Idle;
			Idle.SetHealth(Conf.MaxHealth);
		}

		// Update is called once per frame
		void Update()
		{
			Debug.Assert(CurrentState != null);
			// Delegate to the state update method
			CurrentState.Update(this);
		}

		// Handle collision triggers
		private void OnTriggerEnter2D(Collider2D collision)
		{
			CurrentState.OnTriggerEnter2D(this, collision);
		}

		public void Notify(GameEvent e, int value)
		{
			// Notify all observers of an event
			foreach (IObserver observer in Observers)
			{
				observer.OnNotify(e, value);
			}
		}
	}
}