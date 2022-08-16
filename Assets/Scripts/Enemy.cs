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

	[SerializeField] private GameObject _laserVerticalEnemyPrefab;
    [SerializeField] private GameObject _laserHorizontalEnemyPrefab;

    [SerializeField] private int _typeMovement;

	private Vector3 _offsetLaserVert;
	private Vector3 _offsetLaserHoriz;

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
		SpawnPosition();

		_playerAlive = true;
		_isThisEnemyAlive = true;

		_offsetLaserVert = new Vector3(0, -1.08f, 0);
        _offsetLaserHoriz = new Vector3(0.7f, 0, 0);

		StartCoroutine(FireEnemyRoutine());
	}

	void Update()
    {
		if (_playerAlive == true)
        {
			Movement();
		}
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

    private void SpawnPosition()
    {
		_typeMovement = UnityEngine.Random.Range(0, 2);

		float randomX;
		float ramdomY;
		Vector3 spawnPos;

		switch (_typeMovement)
		{
			case 0: //vertical movement
				randomX = UnityEngine.Random.Range(-9.33f, 9.33f);
				spawnPos = new Vector3(randomX, 7.0f, 0);
				transform.position = spawnPos;
				break;
			case 1: //horizontal movement
				ramdomY = UnityEngine.Random.Range(-1.19f, 5.5f);
				spawnPos = new Vector3(-11.15f, ramdomY, 0);
				transform.position = spawnPos;
				break;
		}
	}

	private void Movement()
    {
		if (_typeMovement == 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
			transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if (transform.position.y < -5.5f && _isThisEnemyAlive == true)
			{
				SpawnPosition();
            }
		}
		else if (_typeMovement == 1)
        {
			transform.rotation = Quaternion.Euler(0, 0, 90);
			transform.Translate(Vector3.down * _speed * Time.deltaTime);

			if (transform.position.x > 11.24 && _isThisEnemyAlive == true)
            {
                SpawnPosition();
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
        while (_isThisEnemyAlive == true && _playerAlive)
		{
			float _wait = UnityEngine.Random.Range(0, 7);

			yield return new WaitForSeconds(_wait);

			switch (_typeMovement)
            {
				case 0:
					Instantiate(_laserVerticalEnemyPrefab, transform.position + _offsetLaserVert, Quaternion.identity);
                    break;
				case 1:
                    Instantiate(_laserHorizontalEnemyPrefab, transform.position + _offsetLaserHoriz, transform.rotation * Quaternion.identity);
					break;
            }
		}
	}

	private void OnDisable()
	{
		Player.OnDeath -= PlayerDeath;
	}
}
