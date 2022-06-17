using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	private WaitForSeconds _wait5sec = new WaitForSeconds(5.0f);

	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private GameObject _enemyContainer;

	[SerializeField] private bool _playerAlive = true;

	private void OnEnable()
	{
		Player.OnDeath += PlayerDead;
	}

	void Start()
	{
		StartCoroutine("SpawnEnemiesRoutine");
	}

	private void PlayerDead()
	{
		//StopCoroutine("SpawnEnemiesRoutine");
		_playerAlive = false;
	}

	IEnumerator SpawnEnemiesRoutine()
	{
		while (_playerAlive == true)
		{
			GameObject newEnemy = Instantiate(_enemyPrefab);
			newEnemy.transform.parent = _enemyContainer.transform;
			yield return _wait5sec;
		}
	}

	private void OnDisable()
	{
		Player.OnDeath -= PlayerDead;
	}
}
