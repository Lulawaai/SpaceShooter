using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BigEnemy : MonoBehaviour
{
	[Header("Basics")]
	[SerializeField] private float _speed;
	[SerializeField] private int _lives;
	[SerializeField] private bool _isAlive;
	[SerializeField] private int _scoreEnemyKilling;
	[SerializeField] private bool _gameOver;
	[SerializeField] private GameObject _laserPrefab;

	[Header("To Destroy this")]
	[SerializeField] private GameObject _explosionPrefab;
	[SerializeField] private Renderer _rend;
	[SerializeField] private GameObject _smallExploPrefab;

	private WaitForSeconds _wait04Sec = new WaitForSeconds(0.4f);
	private WaitForSeconds _wait05Sec = new WaitForSeconds(0.5f);

	private bool _readyToStart;
	private bool _moveLeft;

	public static event Action OnbigEnemyExplosion;
	public static event Action OnBigEnemyDead;
	public static event Action<int> OnEnemyDeadScore;

	private void Start()
	{
		_gameOver = false;
		_moveLeft = RandomLeftOrRight();
		_readyToStart = false;
		_isAlive = true;
		_rend.enabled = true;

		SpawnPosition();
		StartCoroutine(FireRoutine());
	}

	private void Update()
	{
		GeneralMovement();
	}

	private void SpawnPosition()
	{
		float randomX = UnityEngine.Random.Range(-7.4f, 7.4f);
		transform.position = new Vector3(randomX, 10, 0);
	}

	private bool RandomLeftOrRight()
	{
		int i = UnityEngine.Random.Range(0, 2);
		if (i > 0.5f)
		{
			return true;
		}
		else
			return false;
	}

	#region movement
	private void GeneralMovement()
	{
		if (transform.position.y > 4)
		{
			transform.Translate(Vector2.down * Time.deltaTime * _speed);
		}
		else
			StartCoroutine(ReadyToStartRoutine());

		if (_readyToStart == true)
		{
			MoveLeftRight();
			Move();
		}
	}
	private void MoveLeftRight()
	{
		if (transform.position.x > 7.4f)
		{
			_moveLeft = true;
		}

		else if (transform.position.x < -7.4f)
		{
			_moveLeft = false;
		}
	}

	private void Move()
	{
		if (_moveLeft == false)
		{
			transform.Translate(Vector2.right * _speed * Time.deltaTime);
		}

		else
		{
			transform.Translate(Vector2.left * _speed * Time.deltaTime);
		}
	}

	IEnumerator ReadyToStartRoutine()
	{
		yield return _wait04Sec;
		_readyToStart = true;
	}
	#endregion

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Laser"))
		{
			if (_lives > 1)
			{
				_lives--;
				GameObject smallExplosion = Instantiate(_smallExploPrefab, other.transform.position, Quaternion.identity);
				smallExplosion.transform.parent = gameObject.transform;

				OnbigEnemyExplosion?.Invoke();//AudioManager audio explosion

 				Destroy(smallExplosion.gameObject, 2f);
				Destroy(other.gameObject);
			}

			else
			{
				Death();
			}
		}
	}

	IEnumerator FireRoutine() 
	{
		yield return _wait05Sec;

		while (_isAlive)
		{
			float i = UnityEngine.Random.Range(0.7f, 3f);
			GameObject laser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
			laser.transform.parent = gameObject.transform;
			yield return new WaitForSeconds(i);
		}
	}

	private void GameOver()
	{
		_gameOver = true;
	}
	private void Death()
	{
		_isAlive = false;
		OnBigEnemyDead?.Invoke(); //audiomanagerExploSound  //GamemanagerEndGame 
		OnEnemyDeadScore?.Invoke(_scoreEnemyKilling); //UI
		Destroy(this.gameObject, 2.0f);
		_rend.enabled = false;
		_explosionPrefab.SetActive(true);
	}
}
