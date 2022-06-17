using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
	private bool _playerAlive = true;

	private void OnEnable()
	{
		Player.OnDeath += PlayerDeath;
	}
	private void Start()
	{
		float randomX = Random.Range(-9.33f, 9.33f);
		Vector3 spawnPos = new Vector3(randomX, 7.0f, 0);

		transform.position = spawnPos;
	}

	void Update()
    {
		Movement();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Player player = other.transform.GetComponent<Player>();
			if (player != null)
			{
				other.transform.GetComponent<Player>().Damage();
			}
			Destroy(gameObject);
		}

		else if (other.CompareTag("Laser"))
		{
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}

	private void Movement()
	{
		if (_playerAlive == true)
		{
			transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if (transform.position.y < -5.5f)
			{
				float randomX = Random.Range(-9.33f, 9.33f);
				Vector3 spawnPos = new Vector3(randomX, 7.0f, 0);

				transform.position = spawnPos;
			}
		}
	
	}

	private void PlayerDeath()
	{
		_playerAlive = false;
	}

	private void OnDisable()
	{
		Player.OnDeath -= PlayerDeath;
	}
}
