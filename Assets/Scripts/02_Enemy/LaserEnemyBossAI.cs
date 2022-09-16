using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LaserEnemyBossAI : MonoBehaviour
{
	[SerializeField] protected float _speed;

    protected Vector3 _endPos = new Vector3(0, 0, 0);

	protected virtual void Update()
	{
		MoveTowardsCenter();
    }

    protected void MoveTowardsCenter()
    {
        transform.position = Vector3.MoveTowards(transform.position, _endPos, _speed * Time.deltaTime);

        if (transform.position == _endPos)
        {
            if (transform.parent != null)
            {
				Destroy(transform.parent.gameObject, 0.05f);
			}
            Destroy(gameObject, 0.05f);
        }
	}
}
