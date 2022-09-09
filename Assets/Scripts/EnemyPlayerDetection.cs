using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyPlayerDetection : MonoBehaviour
{
	[SerializeField] private Collider2D _collider;

	public static event Action<Transform, GameObject> OnPlayerDetection;
	public static event Action<GameObject> OnPlayerOut;
	public static event Action<GameObject, Transform> OnLaserDetection;

	private void OnEnable()
	{
		Enemy.OnEnemyDeathShield += ColliderOff;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OnPlayerDetection?.Invoke(other.transform, gameObject.transform.parent.gameObject);
		}

		if (other.CompareTag("Laser"))
		{
			OnLaserDetection?.Invoke(gameObject.transform.parent.gameObject, other.transform); //to enemy
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OnPlayerOut?.Invoke(gameObject.transform.parent.gameObject);
		}
	}

	private void ColliderOff(GameObject shield)
	{
		if (gameObject == shield)
		{
			_collider.enabled = false;
		}
	}

	private void OnDisable()
	{
		Enemy.OnEnemyDeathShield -= ColliderOff;
	}
}
