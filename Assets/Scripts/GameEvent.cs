using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
	// Events that are observed by the observer
	public enum GameEvent
	{
		PLAYER_ATTACK,
		PLAYER_DASH,
		PLAYER_TAKE_DAMAGE,
		PLAYER_DEATH,
		COIN_COLLECTED
	};
}