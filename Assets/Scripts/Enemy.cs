using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;

    void Update()
    {
		Movement();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Player player = other.transform.GetComponent<Player>();
			if (player != null)
			{
				other.transform.GetComponent<Player>().Damage();
			}
			Destroy(gameObject);
		}

		else if (other.CompareTag("Laser"))
		{
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}

	private void Movement()
	{
		transform.Translate(Vector3.down * _speed * Time.deltaTime);

		if (transform.position.y < -5.5f)
		{
			float randomX = Random.Range(-9.33f, 9.33f);
			Vector3 spawnPos = new Vector3(randomX, 7.0f, 0);

			transform.position = spawnPos;
		}
	}
}
