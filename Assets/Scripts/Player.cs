using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private float _speed;
	private Vector2 _direction;

	[Header("Laser")]
	[SerializeField] private Transform _laserPrefab;
	[SerializeField] private Vector3 _laserOffset = new Vector3(0, 1f, 0);
	[SerializeField] private float _fireRate = 0.5f;
	private float _nextFire;

	private void OnEnable()
	{
		GameInput.OnFire += Fire;
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

	private void Fire()
	{
		if(Time.time > _nextFire)
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
