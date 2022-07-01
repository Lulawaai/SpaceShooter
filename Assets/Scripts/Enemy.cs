using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
	[SerializeField] private Animator _anim;
	[SerializeField] private Rigidbody2D _rb;

	[SerializeField] private float _enemyExploTime;

	private bool _playerAlive = true;
	private bool _isThisEnemyAlive = true;

	public static event Action<int> OnEnemyDeathLaser;
	public static event Action OnEnemyDeathPlayer;

	private void OnEnable()
	{
		Player.OnDeath += PlayerDeath;
	}
	private void Start()
	{
		float randomX = UnityEngine.Random.Range(-9.33f, 9.33f);
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
			_isThisEnemyAlive = false;
			OnEnemyDeathPlayer?.Invoke();
			EnemyDeath();
		}

		else if (other.CompareTag("Laser"))
		{
			_isThisEnemyAlive = false;
			int score = UnityEngine.Random.Range(10, 30);
			Destroy(other.gameObject);
			OnEnemyDeathLaser?.Invoke(score);
			EnemyDeath();
		}
	}

	private void Movement()
	{
		if (_playerAlive == true)
		{
			transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if (transform.position.y < -5.5f && _isThisEnemyAlive == true)
			{
				float randomX = UnityEngine.Random.Range(-9.33f, 9.33f);
				Vector3 spawnPos = new Vector3(randomX, 7.0f, 0);

				transform.position = spawnPos;
			}
		}
	}

	private void PlayerDeath()
	{
		_playerAlive = false;
	}

	private void EnemyDeath()
	{
		_anim.SetBool("OnEnemyDeath", true);

		//The rigidbody has to be disable
		//the enemy cannot interact while dying.
		_rb.isKinematic = true;
		_rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
		Destroy(this.gameObject, _enemyExploTime);
	}

	private void OnDisable()
	{
		Player.OnDeath -= PlayerDeath;
	}
}
