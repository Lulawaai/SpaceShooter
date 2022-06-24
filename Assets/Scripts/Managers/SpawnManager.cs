using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	private WaitForSeconds _wait1sec = new WaitForSeconds(10.0f);

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
			yield return _wait1sec;
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
