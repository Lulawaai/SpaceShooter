using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySmart : MonoBehaviour
{
	public static event Action<GameObject> OnSmartFireDetection;
	public static event Action<GameObject> OnSmartFireFinished;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OnSmartFireDetection?.Invoke(gameObject.transform.parent.gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			OnSmartFireFinished?.Invoke(gameObject.transform.parent.gameObject);
		}
	}
}
