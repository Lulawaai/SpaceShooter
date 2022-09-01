using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
	[Header("Basic Player")]
	[SerializeField] private bool _isAlive;
	[SerializeField] private float _speed;
	[SerializeField] private float _normalSpeed;
	[SerializeField] private float _acceleration;
	[SerializeField] private int _lives;
	[SerializeField] private int _score;
	[SerializeField] private int _numberShots;
	[SerializeField] private bool _isOutofShots;
	private Vector2 _direction;

	[Header("Damage")]
	[SerializeField] private GameObject _damageRight;
	[SerializeField] private GameObject _damageLeft;
	[SerializeField] private float _damageShakeAmount;
	[SerializeField] private float _damageShakeDuration;

	[Header("Laser")]
	[SerializeField] private Transform _laserPrefab;
	[SerializeField] private Transform _tripleLaserPrefab;
	[SerializeField] private Vector3 _laserOffset = new Vector3(0, 1.14f, 0);
	[SerializeField] private Vector3 _3laserOffset = new Vector3(0, -3.35f, 0);
	[SerializeField] private float _fireRate = 0.5f;
	[SerializeField] private bool _isTripleShotActive = false;
	private float _nextFire;
	private WaitForSeconds _wait5secs = new WaitForSeconds(5.0f);

    //
    private int _speedSituations = 0;

	[Header("Speed")]
	[SerializeField] private float _speedPowerUP;
	[SerializeField] private bool _isSpeepPowerUPActive = false;

	[Header("Shield")]
	[SerializeField] private bool _isShieldOn = false;
	[SerializeField] private GameObject[] _shieldGo;
	[SerializeField] private int _shieldNumber = 3;

	[Header("ExtraFire")]
	[SerializeField] private bool _isExtraFire = false;
    [SerializeField] private GameObject _extraFirePrefab;

    [Header("Slow")]
	[SerializeField] private float _speedSlow;
	private WaitForSeconds _wait3secs = new WaitForSeconds(3.0f);

	public static event Action OnDeath;
    public static event Action<int> OnLossingLives;
    public static event Action<int> OnPlayerFiring;
    public static event Action OnExplosion;
    public static event Action OnPowerUp;
    public static event Action OnOutofShots;
	public static event Action<float, float> OnDamageCameraShake;

	private void OnEnable()
	{
		GameInput.OnFire += Fire;
		GameInput.OnSpeedingUP += Acceleration;
		GameInput.OnSetBackNormalSpeed += Decceleration;
		PowerUp.OnPlayerHit_TripleLaser += TripleShotPowerUP;
		PowerUp.OnPlayerHit_Speed += SpeedPowerUP;
        PowerUp.OnPlayerHit_Shield += ShieldPowerUP;
        PowerUp.OnPlayerHit_FireRefill += FireRefill;
        PowerUp.OnPlayerHit_Health += HealthRefill;
        PowerUp.OnPlayerHit_ExtraFire += ExtraFire;
		PowerUp.OnPlayerHit_Slow += SlowDown;
		Enemy.OnEnemyDeathPlayer += Damage;
		LaserEnemy.OnPlayerDamage += Damage;
        LaserEnemyHori.OnPlayerDamage += Damage;
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

		if (_numberShots == 0 && _isOutofShots == false)
        {
            OnOutofShots?.Invoke(); //to play out of shot sound
            _isOutofShots = true;
        }
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
        _direction = move;

		//_speedSituations
		//normal = 0
		//SpeedUP = 1
		//Acceleration = 2
		//Slow = 3

		switch(_speedSituations)
        {
			case 0:
				_speed = _normalSpeed;
				break;
			case 1:
				_speed = _speedPowerUP;
				StartCoroutine(SpeedUPRoutine());
				break;
            case 2:
                _speed += _acceleration;
                if (_speed > 30)
                {
                    _speed = 30;
                }
                break;
            case 3:
                _speed = _speedSlow;
                break;
			default:
				Debug.Log("Player::: Speed situation default case");
				break;
		}

        transform.Translate(_direction * _speed * Time.deltaTime);
	}

	private void Acceleration()
	{
        _speedSituations = 2;
	}

	private void Decceleration()
	{
        _speedSituations = 0;
	}

	private void SlowDown()
    {
        _speedSituations = 3;
        StartCoroutine(SlowBackRoutine());
    }

	IEnumerator SlowBackRoutine()
    {
		yield return _wait3secs;
		_speedSituations = 0;
    }

    #endregion

    private void Damage()
	{
		if (_isShieldOn == true)
		{
            if (_shieldNumber > 0)
            {
				_shieldGo[_shieldNumber - 1].SetActive(false);
				_shieldNumber -= 1;
            }
			else
                _isShieldOn = false;

			return;
		}

		_lives--;
		OnExplosion?.Invoke();
        OnDamageCameraShake?.Invoke(_damageShakeAmount, _damageShakeDuration);
		OnLossingLives?.Invoke(_lives);

		PlayerDamageUpdate();

		if (_lives < 1)
		{
			_lives = 0;
			_isAlive = false;
			OnDeath?.Invoke();
			Destroy(gameObject);
		}
	}

    private void PlayerDamageUpdate()
    {
        switch (_lives)
        {
            case 3:
                _damageLeft.SetActive(false);
                _damageRight.SetActive(false);
				break;
            case 2:
                _damageLeft.SetActive(true);
                _damageRight.SetActive(false);
                break;
            case 1:
                _damageRight.SetActive(true);
                break;
        }
	}

    private void Fire()
	{
        if (Time.time > _nextFire && _numberShots > 0)
		{
			_nextFire = Time.time + _fireRate;

			if (_isExtraFire == false)
            {
                Debug.Log("isExtraFire " + _isExtraFire);
				if (_isTripleShotActive == true)
				{
					Instantiate(_tripleLaserPrefab, transform.position + _3laserOffset, Quaternion.identity);
                    _numberShots -= 1;
					OnPlayerFiring?.Invoke(_numberShots); //to play fire sound audioManager && UIManager remove fire
					StartCoroutine(TripleLaserCoroutine());
				}
				else
				{
					Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
					_numberShots -= 1;
					OnPlayerFiring?.Invoke(_numberShots); //to play fire sound audioManager && UIManager remove fire
				}
			}
			else
            {
                Instantiate(_extraFirePrefab, transform.position + _laserOffset, Quaternion.identity);
				_numberShots -= 1;
				OnPlayerFiring?.Invoke(_numberShots); //to play fire sound audioManager && UIManager remove fire
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
		_speedSituations = 1;
		OnPowerUp?.Invoke();
	}

	IEnumerator SpeedUPRoutine()
	{
		yield return _wait5secs;
        _speedSituations = 0;
		_isSpeepPowerUPActive = false;
	}

	private void ShieldPowerUP()
	{
		_isShieldOn = true;
		OnPowerUp?.Invoke();

		int i = 0;
		while(i < 3)
        {
			_shieldGo[i].SetActive(true);
            i++;
        }
	}

	private void ExtraFire()
    {
		_isExtraFire = true;
        StartCoroutine(ExtraFireRoutine());
    }

	IEnumerator ExtraFireRoutine()
    {
        yield return _wait5secs;
		_isExtraFire = false;
    }

	private void FireRefill()
    {
        _numberShots = 15;
    }

	private void HealthRefill()
    {
		if (_lives < 3)
        {
			_lives += 1;
            OnLossingLives?.Invoke(_lives); //update UI
            PlayerDamageUpdate();
		}
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
        PowerUp.OnPlayerHit_FireRefill -= FireRefill;
        PowerUp.OnPlayerHit_Health -= HealthRefill;
		PowerUp.OnPlayerHit_ExtraFire -= ExtraFire;
		PowerUp.OnPlayerHit_Slow -= SlowDown;
		Enemy.OnEnemyDeathPlayer -= Damage;
		LaserEnemy.OnPlayerDamage -= Damage;
        LaserEnemyHori.OnPlayerDamage -= Damage;
	}
}
