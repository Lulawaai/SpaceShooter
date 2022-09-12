using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField] private float _speed = 8;

	private void Update()
	{
		Movement();
	}

	private void Movement()
	{
		transform.Translate(Vector3.up * _speed * Time.deltaTime);

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
