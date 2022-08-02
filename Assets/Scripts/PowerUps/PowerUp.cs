using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUp : MonoBehaviour
{
	[SerializeField] private float _speed = 3.0f;

    //0 == TripleLaser, 1 == Speed, 2 == Shield
    [Header("0-TripleLaser, 1-Speed, 2-Shield")]
    [SerializeField] int _powerUpID;

    public static event Action OnPlayerHit_TripleLaser;
    public static event Action OnPlayerHit_Speed;
    public static event Action OnPlayerHit_Shield;

    private void Start()
    {
        float randomX = UnityEngine.Random.Range(-9.33f, 9.33f);
        Vector3 spawnPos = new Vector3(randomX, 7.0f, 0);

        transform.position = spawnPos;
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
		{
			switch (_powerUpID)
			{
				case 0:
					OnPlayerHit_TripleLaser?.Invoke();
					break;
				case 1:
					OnPlayerHit_Speed?.Invoke();
					break;
				case 2:
					OnPlayerHit_Shield?.Invoke();
					break;
				default:
					Debug.Log("default case");
					break;
			}

			Destroy(this.gameObject);
		}
		else if (other.CompareTag("Enemy") || other.CompareTag("Laser"))
		{
			Destroy(this.gameObject);
		}
	}
	private void Move()
	{
		transform.Translate(Vector3.down * _speed * Time.deltaTime);

		if (transform.position.y < -6.0f)
		{
			Destroy(this.gameObject);
		}
	}
}
