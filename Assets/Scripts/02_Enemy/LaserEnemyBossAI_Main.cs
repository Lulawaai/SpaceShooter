using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LaserEnemyBossAI_Main : LaserEnemyBossAI
{
	private bool _arrivedEndPos = false;

	public static event Action OnlaserReachedEndPos; 

	protected override void Update()
	{
		base.Update(); //Execute everything from LaserEnemyBossAI class

		//Plus this extra
		if (gameObject.transform.position == _endPos && _arrivedEndPos == false)
		{
			_arrivedEndPos = true;
			OnlaserReachedEndPos?.Invoke();
		}
	}
}
