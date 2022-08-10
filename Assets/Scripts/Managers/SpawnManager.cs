using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private float _enemyTimeSpawn = 0.1f;
	private WaitForSeconds _waitEnemyTime;
	private WaitForSeconds _wait2Secs = new WaitForSeconds(2.0f);

	[SerializeField] private bool _playerAlive = true;

	[Header("Enemies")]
	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private GameObject _enemyContainer;

	[Header("Power UPs")]
	[SerializeField] private GameObject _powerUpContainer;
	[SerializeField] private GameObject[] _powerUPPrefab;
    [SerializeField] private int _numberPowerUps;

	private void OnEnable()
	{
		Player.OnDeath += PlayerDead;
		Asteroid.OnAsteroidDestroyed += StartSpawning;
	}

	void Start()
	{
		_waitEnemyTime = new WaitForSeconds(_enemyTimeSpawn);
	}

	private void PlayerDead()
	{
		_playerAlive = false;
	}

	private void StartSpawning()
	{
		StartCoroutine(SpawnEnemiesRoutine());
		StartCoroutine(SpawnPowerUPRoutine());
        StartCoroutine(ExtraFireRoutine());
	}

	IEnumerator SpawnEnemiesRoutine()
	{
		yield return _wait2Secs;
		while (_playerAlive == true)
		{
			GameObject newEnemy = Instantiate(_enemyPrefab);
			newEnemy.transform.parent = _enemyContainer.transform;
			yield return _waitEnemyTime;
		}
	}

	IEnumerator SpawnPowerUPRoutine()
	{
		yield return _wait2Secs;
		while (_playerAlive == true)
		{
            int randomPowerUP = Random.Range(0, _numberPowerUps);
            GameObject newPower = Instantiate(_powerUPPrefab[randomPowerUP]);
            newPower.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(Random.Range(0f, 3f));
		}
	}

	IEnumerator ExtraFireRoutine()
    {
        yield return _wait2Secs;
		if (_playerAlive == true)
        {
			yield return new WaitForSeconds(Random.Range(4f, 6f));
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
