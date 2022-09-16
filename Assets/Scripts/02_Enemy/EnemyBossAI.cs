using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBossAI : MonoBehaviour
{
	[Header("Basic")]
	[SerializeField] private float _speed;
	[SerializeField] private bool _isGameOver = false;
	[SerializeField] private int _bossAILifes = 5;
	[SerializeField] private int _bossPoints;
	private Vector3 _startPos = new Vector3(0, 10, 0);
	private Vector3 _endPos = new Vector3 (0, 3.5f, 0);
	private Vector3 _laserOffset = new Vector3(0, -2, 0);
	private Vector3 _laserBallPos = new Vector3(0, 0 ,0);


	[Header("Fire")]
	[SerializeField] private GameObject _firePrefab;
	[SerializeField] private GameObject _fireBallPrefab;

	[Header("To Destroy this")]
	[SerializeField] private GameObject _smallExplosion;
	[SerializeField] private GameObject _bigExplosion;
	[SerializeField] private Renderer _rend;
	private Transform _laserHitPos;

	[Header("Shake")]
	[SerializeField] private float _damageShakeDuration;

	private WaitForSeconds _wait2secs = new WaitForSeconds(2.0f);

	public static event Action<int> OnBossAIHit;
	public static event Action OnBossAIHitPlaySound;
	public static event Action OnBossAIDeath;
 
	private void OnEnable()
	{
		LaserEnemyBossAI_Main.OnlaserReachedEndPos += FireBall;
		Player.OnDeath += GameOver;
	}

	private void Start()
	{
		transform.position = _startPos;
		_isGameOver = false;
		StartCoroutine(FireRoutine());
	}

	private void Update()
	{
		MoveTowardsCenter();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Laser") && !_isGameOver)
		{
			_laserHitPos = other.transform;

			if (_bossAILifes > 0)
			{
				_bossAILifes--;
				bossAIHit();
				Destroy(other.gameObject);
			}
			else if (_bossAILifes == 0)
			{
				_isGameOver = true;
				Destroy(other.gameObject);
				bossAIHit();
			}
		}
	}

	private void MoveTowardsCenter()
	{
		if (transform.position != _endPos)
		{
			transform.position = Vector3.MoveTowards(transform.position, _endPos, _speed * Time.deltaTime);
		}
	}

	IEnumerator FireRoutine()
	{
		yield return _wait2secs;

		while (_isGameOver == false)
		{
			GameObject laser = Instantiate(_firePrefab, transform.position + _laserOffset, Quaternion.identity);
			laser.transform.parent = transform;
			float i = UnityEngine.Random.Range(0.2f, 1.2f);
			yield return new WaitForSeconds(i);
		}
	}

	private void FireBall()
	{
		if (_isGameOver == false)
		{
			GameObject fireBall = Instantiate(_fireBallPrefab, _laserBallPos, Quaternion.identity);
			fireBall.transform.parent = transform;
		}
	}

	private void bossAIHit()
	{
		if (!_isGameOver)
		{
			_bossPoints = UnityEngine.Random.Range(20, 60);
			DamageShake();
			GameObject smallExplo = Instantiate(_smallExplosion, _laserHitPos.position, Quaternion.identity);
			OnBossAIHit?.Invoke(_bossPoints);
			OnBossAIHitPlaySound?.Invoke();
			Destroy(smallExplo, 1f);
		}
		else
		{
			_bossPoints = 200;
			DamageShake();
			GameObject finalExplosion = Instantiate(_bigExplosion, transform.position, Quaternion.identity);
			finalExplosion.transform.parent = transform;
			_rend.enabled = false;
			OnBossAIHit?.Invoke(_bossPoints);
			OnBossAIHitPlaySound?.Invoke();
			OnBossAIDeath?.Invoke();
			Destroy(gameObject, 2f);
		}
	}

	private void DamageShake()
	{
		StartCoroutine(ShakeRoutine());
	}

	private IEnumerator ShakeRoutine()
	{
		float timeElapsed = 0;

		while (timeElapsed < _damageShakeDuration)
		{
			float xOffset = UnityEngine.Random.Range(0.2f, -0.2f);
			float yOffset = UnityEngine.Random.Range(3.4f, 3.6f);

			transform.position = new Vector3(xOffset, yOffset, 0);

			timeElapsed += Time.deltaTime;

			yield return null;
		}

		transform.position = _endPos;
	}

	private void GameOver()
	{
		_isGameOver = true;
	}

	private void OnDisable()
	{
		LaserEnemyBossAI_Main.OnlaserReachedEndPos -= FireBall;
		Player.OnDeath += GameOver;
	}
}
