using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyDetectPickUp : MonoBehaviour
{
	public static event Action<GameObject> OnDetectingPowerUp;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("PowerUp"))
		{
			OnDetectingPowerUp?.Invoke(gameObject.transform.parent.gameObject);
		}
	}
}
