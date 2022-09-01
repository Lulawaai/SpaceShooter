using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private float _enemyTimeSpawn = 0.1f;
	private WaitForSeconds _waitEnemyTime;
	private WaitForSeconds _wait2Secs = new WaitForSeconds(2.0f);

	[SerializeField] private bool _gameRunning = true;

	[Header("Enemies")]
	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject _bigEnemy;
	[SerializeField] private int _nrEmenies;
	[SerializeField] private int _nrEmeniesActive;
	[SerializeField] private int _waveNr;
	[SerializeField] private int _timeBetWave;
	[SerializeField] private int _numberExtraEnemies;


	[Header("Power UPs")]
	[SerializeField] private GameObject _powerUpContainer;
	[SerializeField] private GameObject[] _powerUPPrefab;
	[SerializeField] private GameObject[] _powerHealthFirePrefab;

	public static event Action<int> OnUpdatingWaveNr;

	private void OnEnable()
	{
		Player.OnDeath += GameOver;
		BigEnemy.OnBigEnemyDead += GameOver;
		Asteroid.OnAsteroidDestroyed += StartSpawning;
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

		if (_waveNr < 3)
		{
			StartCoroutine(SpawnEnemiesRoutine());
		}
		else
			Instantiate(_bigEnemy);
	}

	IEnumerator SpawnPowerUPRoutine()
	{
		yield return _wait2Secs;
		while (_gameRunning == true)
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
		if (_gameRunning == true)
        {
			yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 6f));
			GameObject firePower = Instantiate(_powerUPPrefab[2]);
            firePower.transform.parent = _powerUpContainer.transform;
		}
    }

	private void OnDisable()
	{
		Player.OnDeath -= GameOver;
		BigEnemy.OnBigEnemyDead -= GameOver;
		Asteroid.OnAsteroidDestroyed -= StartSpawning;
	}
}
