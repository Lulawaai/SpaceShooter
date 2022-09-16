using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserBallBossAI : MonoBehaviour
{
	[SerializeField] private GameObject _playerGO;

	[Header("Basics")]
	[SerializeField] private float _speed;
	private WaitForSeconds _wait03sec = new WaitForSeconds(0.3f);

	[Header("Colliders")]
	[SerializeField] private Collider2D _detectFollowPlayer;
	[SerializeField] private bool _followingPlayer;
	[SerializeField] private Collider2D _detectKillPlayer;

	public static event Action OnkillingPlayer;

	private void OnEnable()
	{
		_playerGO = null;
		StartCoroutine(AutoDestroyRoutine());
	}

	private void Start()
	{
		_detectFollowPlayer.enabled = true;
		_detectKillPlayer.enabled = false;
	}

	private void Update()
	{
		if (_playerGO != null)
		{
			float minDist = 0.1f;
			float dist = Vector3.Distance(transform.position, _playerGO.transform.position);

			if (dist > minDist)
			{
				transform.position = Vector3.MoveTowards(transform.position, _playerGO.transform.position, _speed * Time.deltaTime);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (_followingPlayer == false)
			{
				_playerGO = other.gameObject;
				_detectFollowPlayer.enabled = false;
				_detectKillPlayer.enabled = true;
				_followingPlayer = true;
			}

			else
			{
				OnkillingPlayer?.Invoke();
				Destroy(gameObject, 0.3f);
			}
		}

		else if (other.CompareTag("Laser"))
		{
			Destroy(other.gameObject);
			Destroy(gameObject, 0.1f);
		}
	}

	IEnumerator AutoDestroyRoutine()
	{
		yield return _wait03sec;
		Destroy(gameObject);
	}
}
