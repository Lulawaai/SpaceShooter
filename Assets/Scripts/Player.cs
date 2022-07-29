using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [Header("Basic Player")]
    [SerializeField] private bool _isAlive;
    [SerializeField] private float _speed;
    [SerializeField] private bool _isSpeedUP = false;
    [SerializeField] private float _normalSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private int _lives;
    [SerializeField] private GameObject _shieldGo;
    [SerializeField] private int _score;
    private Vector2 _direction;

    [Header("Damage")]
    [SerializeField] private GameObject _damageRight;
    [SerializeField] private GameObject _damageLeft;

	[Header("Laser")]
	[SerializeField] private Transform _laserPrefab;
	[SerializeField] private Transform _tripleLaserPrefab;
	[SerializeField] private Vector3 _laserOffset = new Vector3(0, 1.14f, 0);
	[SerializeField] private Vector3 _3laserOffset = new Vector3(0, -3.35f, 0);
	[SerializeField] private float _fireRate = 0.5f;
	[SerializeField] private bool _isTripleShotActive = false;
	private float _nextFire;
	private WaitForSeconds _wait5secs = new WaitForSeconds(5.0f);

	[Header("Speed")]
	[SerializeField] private float _speedPowerUP;
	[SerializeField] private bool _isSpeepPowerUPActive = false;

	[Header("Shield")]
	[SerializeField] private bool _isShieldOn = false;

	public static event Action OnDeath;
	public static event Action<int> OnLossingLives;
	public static event Action OnPlayerFiring;
	public static event Action OnExplosion;
	public static event Action OnPowerUp;

	private void OnEnable()
	{
		GameInput.OnFire += Fire;
		GameInput.OnSpeedingUP += Acceleration;
		GameInput.OnSetBackNormalSpeed += Decceleration;
		PowerUp.OnPlayerHit_TripleLaser += TripleShotPowerUP;
		PowerUp.OnPlayerHit_Speed += SpeedPowerUP;
		PowerUp.OnPlayerHit_Shield += ShieldPowerUP;
		Enemy.OnEnemyDeathPlayer += Damage;
		LaserEnemy.OnPlayerDamage += Damage;
	}

	private void Start()
	{
		_speed = _normalSpeed;
		_isAlive = true;
		_lives = 3;
		_damageRight.SetActive(false);
		_damageLeft.SetActive(false);
	}

	private void Update()
	{
		ClampPlayerMove();
	}

	#region PlayerMove
	private void ClampPlayerMove()
	{
		transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 1));

		if (transform.position.x > 11)
		{
			transform.position = new Vector2(-11, transform.position.y);
		}

		else if (transform.position.x < -11)
		{
			transform.position = new Vector2(11, transform.position.y);
		}
	}

    public void Move(Vector2 move)
	{
        //normal speed
        //acceleration
        //power speed

        _direction = move;

        if (_isSpeepPowerUPActive == false && _isSpeedUP == false)
		{
            _speed = _normalSpeed;
        }

        if (_isSpeepPowerUPActive == true && _isSpeedUP == false)
        {
            _speed = _speedPowerUP;
            StartCoroutine(SpeedUPRoutine());
        }

        if (_isSpeedUP == true)
        {
            _speed += _acceleration;

			if (_speed > 30)
            {
				_speed = 30;
            }
        }

		transform.Translate(_direction * _speed * Time.deltaTime);
	}

	private void Acceleration()
	{
        _isSpeedUP = true;
		//_speed += _acceleration;
	}

	private void Decceleration()
	{
		_isSpeedUP = false;
		//_speed = _normalSpeed;
	}

	#endregion

	private void Damage()
	{
		if (_isShieldOn == true)
		{
			_isShieldOn = false;
			_shieldGo.SetActive(false);
			return;
		}

		_lives--;
		OnExplosion?.Invoke();
		OnLossingLives?.Invoke(_lives);

		switch (_lives)
		{
			case 3:
				break;
			case 2:
				_damageLeft.SetActive(true);
				break;
			case 1:
				_damageRight.SetActive(true);
				break;
		}

		if (_lives < 1)
		{
			_lives = 0;
			_isAlive = false;
			OnDeath?.Invoke();
			Destroy(gameObject);
		}
	}

	private void Fire()
	{
		if (Time.time > _nextFire)
		{
			_nextFire = Time.time + _fireRate;

			if (_isTripleShotActive == true)
			{
				Instantiate(_tripleLaserPrefab, transform.position + _3laserOffset, Quaternion.identity);
				OnPlayerFiring?.Invoke();
				StartCoroutine(TripleLaserCoroutine());
			}
			else
			{
				Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
				OnPlayerFiring?.Invoke();
			}
		}
	}

	#region powerUps
	private void TripleShotPowerUP()
	{
		_isTripleShotActive = true;
		OnPowerUp?.Invoke();
	}

	IEnumerator TripleLaserCoroutine()
	{
		yield return _wait5secs;
		_isTripleShotActive = false;
	}

	private void SpeedPowerUP()
	{
		_isSpeepPowerUPActive = true;
		OnPowerUp?.Invoke();
	}

	IEnumerator SpeedUPRoutine()
	{
		yield return _wait5secs;
		_isSpeepPowerUPActive = false;
	}

	private void ShieldPowerUP()
	{
		_isShieldOn = true;
		_shieldGo.SetActive(true);
		OnPowerUp?.Invoke();
	}
	#endregion
	  
	private void OnDisable()
	{
		GameInput.OnFire -= Fire;
		GameInput.OnSpeedingUP += Acceleration;
		GameInput.OnSetBackNormalSpeed += Decceleration;
		PowerUp.OnPlayerHit_TripleLaser -= TripleShotPowerUP;
		PowerUp.OnPlayerHit_Speed -= SpeedPowerUP;
		PowerUp.OnPlayerHit_Shield -= ShieldPowerUP;
		Enemy.OnEnemyDeathPlayer -= Damage;
		LaserEnemy.OnPlayerDamage -= Damage;
	}
}
