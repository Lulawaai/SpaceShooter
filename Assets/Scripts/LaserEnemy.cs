using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaserEnemy : MonoBehaviour
{
    [SerializeField] private float _speed;

    public static event Action OnPlayerDamage;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
		{
			Destroy(gameObject);
		}
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
        if (other.CompareTag("Player"))
		{
			OnPlayerDamage?.Invoke();
			Destroy(gameObject);
		}

        else if (other.CompareTag("Laser"))
		{
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}
