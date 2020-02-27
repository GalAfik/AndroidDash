using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AndroidDash
{
	public interface IObserver
	{
		void OnNotify(GameEvent e, int value);
	}
}