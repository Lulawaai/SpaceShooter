using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private float _enemyTimeSpawn = 0.1f;
	private WaitForSeconds _waitEnemyTime;
	private WaitForSeconds _wait2Secs = new WaitForSeconds(2.0f);

	[SerializeField] private bool _playerAlive = true;

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
    [SerializeField] private int _numberPowerUps;

	public static event Action<int> OnUpdatingWaveNr;

	private void OnEnable()
	{
		Player.OnDeath += PlayerDead;
		Asteroid.OnAsteroidDestroyed += StartSpawning;
	}

	void Start()
	{
		_waitEnemyTime = new WaitForSeconds(_enemyTimeSpawn);
		_nrEmeniesActive = _nrEmenies;
	}

	private void PlayerDead()
	{
		_playerAlive = false;
	}

	private void StartSpawning()
	{
		StartCoroutine(StartSpawningRoutine());
		StartCoroutine(SpawnPowerUPRoutine());
        StartCoroutine(ExtraFireRoutine());
	}

	IEnumerator StartSpawningRoutine()
    {
        yield return _wait2Secs;
        StartCoroutine(SpawnEnemiesRoutine());
	}

	IEnumerator SpawnEnemiesRoutine()
	{
		while (_playerAlive == true && _nrEmeniesActive > 0)
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

		if (_waveNr < 4)
		{
			StartCoroutine(SpawnEnemiesRoutine());
		}

		Instantiate(_bigEnemy);

	}

	IEnumerator SpawnPowerUPRoutine()
	{
		yield return _wait2Secs;
		while (_playerAlive == true)
		{
            int randomPowerUP = UnityEngine.Random.Range(0, _numberPowerUps);
            GameObject newPower = Instantiate(_powerUPPrefab[randomPowerUP]);
            newPower.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 3f));
		}
	}

	IEnumerator ExtraFireRoutine()
    {
        yield return _wait2Secs;
		if (_playerAlive == true)
        {
			yield return new WaitForSeconds(UnityEngine.Random.Range(4f, 6f));
            GameObject firePower = Instantiate(_powerUPPrefab[5]);
            firePower.transform.parent = _powerUpContainer.transform;
		}

    }

	private void OnDisable()
	{
		Player.OnDeath -= PlayerDead;
		Asteroid.OnAsteroidDestroyed -= StartSpawning;
	}
}
