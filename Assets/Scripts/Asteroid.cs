using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
	[SerializeField] private SpriteRenderer _spriteRend;
	[SerializeField] private Animator _animExplo;

	public static event Action OnAsteroidDestroyed;

	void Update()
	{
        transform.Rotate(Vector3.forward * _rotSpeed * Time.deltaTime);
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Laser"))
		{
			OnAsteroidDestroyed?.Invoke();
			Destroy(other.gameObject);
			_spriteRend.enabled = false;
			_animExplo.SetBool("OnAsteroidExplosion", true);
			Destroy(this.gameObject, 2.0f);
		}
	}
}
