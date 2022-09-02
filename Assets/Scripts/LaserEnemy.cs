using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//laser Enemy Vertical
public class LaserEnemy : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] private bool _backFire;

	public static event Action OnPlayerDamage;

	void Update()
	{
		if (_backFire == false)
		{
			transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if (transform.position.y < -6f)
			{
				Destroy(gameObject);
			}
		}
		else if (_backFire == true)
		{
			transform.Translate(Vector3.up * _speed * Time.deltaTime);

			if (transform.position.y > 6f)
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
