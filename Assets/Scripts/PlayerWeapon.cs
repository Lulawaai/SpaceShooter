using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
	[SerializeField] protected float speed;

	protected void Update()
	{
		Movement();
	}

	protected void Movement()
	{
		transform.Translate(Vector3.up * speed * Time.deltaTime);

		if (transform.position.y > 8f)
		{
			if (transform.parent != null)
			{
				Destroy(transform.parent.gameObject);
			}
			Destroy(gameObject);
		}
	}
}
