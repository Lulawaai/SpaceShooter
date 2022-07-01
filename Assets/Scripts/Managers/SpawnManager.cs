using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField] private float _enemyTimeSpawn = 0.1f;
	private WaitForSeconds _waitEnemyTime;

	[SerializeField] private bool _playerAlive = true;

	[Header("Enemies")]
	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private GameObject _enemyContainer;

	[Header("Power UPs")]
	[SerializeField] private GameObject _powerUpContainer;
	[SerializeField] private GameObject[] _powerUPPrefab;

	private void OnEnable()
	{
		Player.OnDeath += PlayerDead;
	}

	void Start()
	{
		_waitEnemyTime = new WaitForSeconds(_enemyTimeSpawn);
		StartCoroutine(SpawnEnemiesRoutine());
		StartCoroutine(SpawnPowerUPRoutine());
	}

	private void PlayerDead()
	{
		_playerAlive = false;
	}

	IEnumerator SpawnEnemiesRoutine()
	{
		while (_playerAlive == true)
		{
			GameObject newEnemy = Instantiate(_enemyPrefab);
			newEnemy.transform.parent = _enemyContainer.transform;
			yield return _waitEnemyTime;
		}
	}

	IEnumerator SpawnPowerUPRoutine()
	{
		while (_playerAlive == true)
		{
			int randomPowerUP = Random.Range(0, 3);
			GameObject newPower = Instantiate(_powerUPPrefab[randomPowerUP]);
			newPower.transform.parent = _powerUpContainer.transform;
			yield return new WaitForSeconds(Random.Range(0f, 3f));
		}
	}

	private void OnDisable()
	{
		Player.OnDeath -= PlayerDead;
	}
}
