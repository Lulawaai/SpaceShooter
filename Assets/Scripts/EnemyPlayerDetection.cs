using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPlayerDetection : MonoBehaviour
{
	[SerializeField] private Collider2D _collider;

	public static event Action<Transform, GameObject> OnPlayerDetection;
	public static event Action<GameObject> OnPlayerOut;

	private void OnEnable()
	{
		Enemy.OnEnemyDeathPlaySound += ColliderOff;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OnPlayerDetection?.Invoke(other.transform, gameObject.transform.parent.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OnPlayerOut?.Invoke(gameObject.transform.parent.gameObject);
		}
	}

	private void ColliderOff()
	{
		_collider.enabled = false;
	}

	private void OnDisable()
	{
		Enemy.OnEnemyDeathPlaySound -= ColliderOff;
	}
}
