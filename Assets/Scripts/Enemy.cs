using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
	[SerializeField] private Animator _anim;
	[SerializeField] private Collider2D _collider;

	[SerializeField] private float _enemyExploTime;

	[SerializeField] private GameObject _laserEnemyPrefab;

	private Vector3 _offsetLaser;

	private bool _playerAlive;
	private bool _isThisEnemyAlive;

	public static event Action<int> OnEnemyDeathLaser;
	public static event Action OnEnemyDeathPlayer;
	public static event Action OnEnemyDeathPlaySound;

	private void OnEnable()
	{
		Player.OnDeath += PlayerDeath;
	}
	private void Start()
	{
		float randomX = UnityEngine.Random.Range(-9.33f, 9.33f);
		Vector3 spawnPos = new Vector3(randomX, 7.0f, 0);

		transform.position = spawnPos;
		_playerAlive = true;
		_isThisEnemyAlive = true;

		_offsetLaser = new Vector3(0, -1.08f, 0);

		StartCoroutine(FireEnemyRoutine());
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
			OnEnemyDeathPlaySound?.Invoke();
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

		_collider.enabled = false;
		Destroy(this.gameObject, _enemyExploTime);
	}

	IEnumerator FireEnemyRoutine()
	{
		while (_isThisEnemyAlive == true)
		{
			float _wait = UnityEngine.Random.Range(0, 7);
			yield return new WaitForSeconds(_wait);
			Instantiate(_laserEnemyPrefab, transform.position + _offsetLaser, Quaternion.identity);
		}

	}

	private void OnDisable()
	{
		Player.OnDeath -= PlayerDeath;
	}
}
