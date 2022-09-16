using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float _speed;
	[SerializeField] private float _speedTowardsPlayer;

	[SerializeField] private Animator _anim;
	[SerializeField] private Collider2D _collider;
	[SerializeField] private GameObject _shieldEnemy;
	[SerializeField] private GameObject _shieldDectorGO;
	private bool _shieldOn;

	[SerializeField] private float _enemyExploTime;

	[Header("LaserPrefabs")]
	[SerializeField] private GameObject _laserVerticalEnemyPrefab;
    [SerializeField] private GameObject _laserHorizontalEnemyPrefab;
	[SerializeField] private GameObject _laserVertBackPrefab;
	[SerializeField] private GameObject _laserHoriBackPrefab;
	[SerializeField] private bool _smartEnemy;
	[SerializeField] private bool _enemyAvoidingShots;
	[SerializeField] private bool _FireSmart = false;
	[SerializeField] private int _typeMovement;
	[SerializeField] private float _smartFiringTime;
	private WaitForSeconds _waitForSecSmartfiring;

	private Vector3 _offsetLaserVert;
	private Vector3 _offsetLasVerBack;
	private Vector3 _offsetLaserHoriz;
	private Vector3 _offsetLaserHorBack;

	private bool _gamePlaying;
	private bool _isThisEnemyAlive;
	[SerializeField] private bool _playerDetected = false;
	private Transform _playerTransform;

	public static event Action<int> OnEnemyDeathLaser;
	public static event Action OnEnemyDeathPlayer;
	public static event Action OnEnemyDeath;
	public static event Action<GameObject> OnEnemyDeathShield;

	private void OnEnable()
	{
		Player.OnDeath += GameOver;
		BigEnemy.OnBigEnemyDead += GameOver;
		EnemyPlayerDetection.OnPlayerDetection += PlayerDetected;
		EnemyPlayerDetection.OnPlayerOut += PlayerOut;
		EnemyPlayerDetection.OnLaserDetection += LaserDetected;
		EnemySmart.OnSmartFireDetection += FireSmart;
		EnemySmart.OnSmartFireFinished += StopSmartFire;
		EnemyDetectPickUp.OnDetectingPowerUp += DestroyPickUp;
	}
	private void Start()
	{
		_waitForSecSmartfiring = new WaitForSeconds(_smartFiringTime);
		IsSmartEnemy();
		ShieldForTheEnemy();
		SpawnPosition();
		IsEnemyAvoidShots();

		_gamePlaying = true;
		_isThisEnemyAlive = true;

		_offsetLaserVert = new Vector3(0, -1.08f, 0);
		_offsetLasVerBack = new Vector3(0, 0.87f, 0);
        _offsetLaserHoriz = new Vector3(0.7f, 0, 0);
		_offsetLaserHorBack = new Vector3(-0.81f, 0, 0);

		StartCoroutine(FireEnemyRoutine());
	}

	void Update()
    {
		if (_gamePlaying == true)
        {
			if (_playerDetected == true)
			{
				float withinRange = 0.1f;
				float dist = Vector3.Distance(transform.position, _playerTransform.position);
				if (dist > withinRange)
				{
					MoveTowardsTarget();
					RotateTowardsTarget();
				}
			}
			else
			{
				Movement();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (_shieldOn == false)
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
		else
		{
			if (other.CompareTag("Player"))
			{
				_isThisEnemyAlive = false;
				OnEnemyDeathPlayer?.Invoke();
				EnemyDeath();
			}

			else if (other.CompareTag("Laser"))
			{
				_shieldOn = false;
				int score = UnityEngine.Random.Range(10, 30);
				Destroy(other.gameObject);
				OnEnemyDeathLaser?.Invoke(score);

				_shieldEnemy.SetActive(false);
			}
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

	private void GameOver()
	{
		_gamePlaying = false;
	}

	private void EnemyDeath()
	{
		_anim.SetBool("OnEnemyDeath", true);
		OnEnemyDeath?.Invoke(); //AudioManager 
		OnEnemyDeathShield?.Invoke(_shieldDectorGO); //EnemyPlayerDetection

		_collider.enabled = false;
		_shieldEnemy.SetActive(false);
		Destroy(this.gameObject, _enemyExploTime);
	}

	IEnumerator FireEnemyRoutine()
	{
        while (_isThisEnemyAlive == true && _gamePlaying)
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
	
	private void ShieldForTheEnemy()
	{
		int i = UnityEngine.Random.Range(0, 2);

		if (i == 1)
		{
			_shieldEnemy.SetActive(true);
			_shieldOn = true;
		}
		else
			_shieldOn = false;
	}

	#region MoveTowardsEnemy
	private void PlayerDetected(Transform player, GameObject enemy)
	{
		if (this.gameObject == enemy)
		{
			_playerTransform = player;
			_playerDetected = true;
		}
	}

	private void PlayerOut(GameObject enemy)
	{
		if (this.gameObject == enemy)
		{
			_playerDetected = false;
		}
	}

	private void MoveTowardsTarget()
	{
		transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, _speedTowardsPlayer * Time.deltaTime);
	}

	private void RotateTowardsTarget()
	{
		Vector3 diff = _playerTransform.position - transform.position;
		var offset = 90;

		//Mathf.Atan2 == Returns the angle in radians whose Tan(Returns the tangent of angle f in radians.) is y/x.
		float angle = Mathf.Atan2(diff.y, diff.x);

		// Mathf.Rad2Deg == Radians-to-degrees conversion constant.
		//This is equal to 360 / (PI * 2).
		transform.rotation = Quaternion.Euler(0f, 0f, (angle * Mathf.Rad2Deg) + offset);
	}
	#endregion

	#region SmartEnemy
	private void IsSmartEnemy()
	{
		int i = UnityEngine.Random.Range(0, 2);

		if (i != 0)
		{
			_smartEnemy = true;
		}
		else
			_smartEnemy = false;
	}

	private void FireSmart(GameObject enemy)
	{
		_FireSmart = true;

		if (enemy == gameObject && _gamePlaying && _smartEnemy)
		{
			StartCoroutine(SmartFireRoutine());
		}
	}

	IEnumerator SmartFireRoutine()
	{
		while (_FireSmart == true)
		{
			switch (_typeMovement)
			{
				case 0:
					Instantiate(_laserVertBackPrefab, transform.position + _offsetLasVerBack, Quaternion.identity);
					break;
				case 1:
					Instantiate(_laserHoriBackPrefab, transform.position + _offsetLaserHoriz, transform.rotation * Quaternion.identity);
					break;
			}
			yield return _waitForSecSmartfiring;
		}
	}

	private void StopSmartFire(GameObject enemy)
	{
		if (this.gameObject == enemy)
		{
			_FireSmart = false;
		}
	}
	#endregion

	#region enemyAvoidShots
	private void IsEnemyAvoidShots()
	{
		int i = UnityEngine.Random.Range(0, 5);

		if (i == 0)
		{
			_enemyAvoidingShots = true;
		}
		else
			_enemyAvoidingShots = false;
	}

	private void LaserDetected(GameObject enemy, Transform laserTrans)
	{
		if (gameObject == enemy && _enemyAvoidingShots)
		{
			float minDist = 0.1f;
			float dist = Vector3.Distance(transform.position, laserTrans.position);
			float speedMoveAway = 10f;

			if (dist > minDist)
			{
				transform.position = Vector2.MoveTowards(transform.position, laserTrans.position, -1 * speedMoveAway * Time.deltaTime);
			}
		}
	}
	#endregion

	private void DestroyPickUp(GameObject enemy)
	{
		if (gameObject == enemy)
		{
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
		Player.OnDeath -= GameOver;
		BigEnemy.OnBigEnemyDead -= GameOver;
		EnemyPlayerDetection.OnPlayerDetection -= PlayerDetected;
		EnemyPlayerDetection.OnPlayerOut -= PlayerOut;
		EnemyPlayerDetection.OnLaserDetection -= LaserDetected;
		EnemySmart.OnSmartFireDetection -= FireSmart;
		EnemySmart.OnSmartFireFinished -= StopSmartFire;
		EnemyDetectPickUp.OnDetectingPowerUp -= DestroyPickUp;
	}
}
