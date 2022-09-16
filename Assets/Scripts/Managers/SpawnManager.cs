using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private float _enemyTimeSpawn = 0.1f;
	private WaitForSeconds _waitEnemyTime;
	private WaitForSeconds _wait2Secs = new WaitForSeconds(2.0f);
	private WaitForSeconds _wait3Secs = new WaitForSeconds(3.0f);

	[SerializeField] private bool _gameRunning = true;
	private bool _lastWave = false;

	[Header("Enemies")]
	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject _bigEnemy;
	[SerializeField] private GameObject _bossAIEnemy;
	[SerializeField] private int _nrEmenies;
	[SerializeField] private int _nrEmeniesActive;
	[SerializeField] private int _waveNr;
	[SerializeField] private int _timeBetWave;
	[SerializeField] private int _numberExtraEnemies;


	[Header("Power UPs")]
	[SerializeField] private GameObject _powerUpContainer;
	[SerializeField] private GameObject[] _powerUPPrefab;
	[SerializeField] private GameObject[] _powerHealthFirePrefab;
	[SerializeField] private GameObject[] _powerUPExtraFire;

	public static event Action<int> OnUpdatingWaveNr;

	private void OnEnable()
	{
		Player.OnDeath += GameOver;
		Asteroid.OnAsteroidDestroyed += StartSpawning;
		EnemyBossAI.OnBossAIDeath += GameOver;
	}

	void Start()
	{
		_waitEnemyTime = new WaitForSeconds(_enemyTimeSpawn);
		_nrEmeniesActive = _nrEmenies;
	}

	private void GameOver()
	{
		_gameRunning = false;
	}

	private void StartSpawning()
	{
		StartCoroutine(StartSpawningRoutine());

		StartCoroutine(SpawnPowerUPRoutine());
		StartCoroutine(SpawnHealtAndFireRoutine());

		StartCoroutine(ExtraFireRoutine());
	}

	IEnumerator StartSpawningRoutine()
    {
        yield return _wait2Secs;
        StartCoroutine(SpawnEnemiesRoutine());
	}

	IEnumerator SpawnEnemiesRoutine()
	{
		while (_gameRunning == true && _nrEmeniesActive > 0)
		{
			_nrEmeniesActive--;
			GameObject newEnemy = Instantiate(_enemyPrefab);
			newEnemy.transform.parent = _enemyContainer.transform;
			yield return _waitEnemyTime;
		}

		_nrEmenies = _nrEmenies + _numberExtraEnemies;
		_nrEmeniesActive = _nrEmenies;
		yield return new WaitForSeconds(_timeBetWave);
		_waveNr++;
        OnUpdatingWaveNr?.Invoke(_waveNr);

		if (_waveNr < 2)
		{
			StartCoroutine(SpawnEnemiesRoutine());
		}
		else if (_waveNr == 2)
		{
			if (_gameRunning == true)
			{
				Instantiate(_bigEnemy);
				StartCoroutine(SpawnEnemiesRoutine());
			}
		}
		else if (_waveNr == 3)
		{
			_lastWave = true;
			Instantiate(_bossAIEnemy);
		}		
	}

	IEnumerator SpawnPowerUPRoutine()
	{
		yield return _wait2Secs;
		while (_gameRunning == true && _lastWave == false)
		{
			int randomPowerUP = UnityEngine.Random.Range(0, _powerUPPrefab.Length);
            GameObject newPower = Instantiate(_powerUPPrefab[randomPowerUP]);
            newPower.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 5f));
		}
	}

	IEnumerator SpawnHealtAndFireRoutine()
	{
		yield return _wait2Secs;

		while (_gameRunning == true)
		{
			int randomPowerUP = UnityEngine.Random.Range(0, _powerHealthFirePrefab.Length);
			GameObject newPower = Instantiate(_powerHealthFirePrefab[randomPowerUP]);
			newPower.transform.parent = _powerUpContainer.transform;
			yield return new WaitForSeconds(UnityEngine.Random.Range(0, 3f));
		}
	}

	IEnumerator ExtraFireRoutine()
	{
        yield return _wait2Secs;
		while (_gameRunning == true && _lastWave == false)
        {
			int randomPowerUP = UnityEngine.Random.Range(0, _powerUPExtraFire.Length);
			yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 6f));
			GameObject firePower = Instantiate(_powerUPExtraFire[randomPowerUP]);
            firePower.transform.parent = _powerUpContainer.transform;
		}
    }

	private void OnDisable()
	{
		Player.OnDeath -= GameOver;
		Asteroid.OnAsteroidDestroyed -= StartSpawning;
		EnemyBossAI.OnBossAIDeath -= GameOver;
	}
}
