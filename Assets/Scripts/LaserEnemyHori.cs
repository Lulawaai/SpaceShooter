using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserEnemyHori : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] private bool _front = true;

	public static event Action OnPlayerDamage;

	void Update()
	{
		if (_front)
		{
			transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if (transform.position.x > 10.4f)
			{
				Destroy(gameObject);
			}
		}
		else if (_front == false)
		{
			transform.Translate(Vector3.up * _speed * Time.deltaTime);

			if (transform.position.x < -10.4f)
			{
				Destroy(gameObject);
			}
		}
	}

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OnPlayerDamage?.Invoke();
			Destroy(gameObject);
		}

		else if (other.CompareTag("Laser"))
		{
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}
