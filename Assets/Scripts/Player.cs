using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
	[Header("Basic Player")]
	[SerializeField] private bool _isAlive;
	[SerializeField] private float _speed;
	[SerializeField] private int _lives = 3;
	private Vector2 _direction;

	[Header("Laser")]
	[SerializeField] private Transform _laserPrefab;
	[SerializeField] private Vector3 _laserOffset = new Vector3(0, 1f, 0);
	[SerializeField] private float _fireRate = 0.5f;
	private float _nextFire;

	public static event Action OnDeath;

	private void OnEnable()
	{
		GameInput.OnFire += Fire;
	}

	private void Start()
	{
		_isAlive = true;
	}

	private void Update()
	{
		ClampPlayerMove();
	}

	#region PlayerMove
	private void ClampPlayerMove()
	{
		transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 1));

		if (transform.position.x > 11)
		{
			transform.position = new Vector2(-11, transform.position.y);
		}

		else if (transform.position.x < -11)
		{
			transform.position = new Vector2(11, transform.position.y);
		}
	}

	public void Move(Vector2 move)
	{
		_direction = move;

		transform.Translate(_direction * _speed * Time.deltaTime);
	}
	#endregion

	public void Damage()
	{
		_lives--;
		if (_lives < 1)
		{
			_lives = 0;
			_isAlive = false;
			OnDeath?.Invoke();
			Destroy(gameObject);
		}
	}

	private void Fire()
	{
		if (Time.time > _nextFire)
		{
			_nextFire = Time.time + _fireRate;
			Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
		}
		
	}

	private void OnDisable()
	{
		GameInput.OnFire -= Fire;
	}
}
