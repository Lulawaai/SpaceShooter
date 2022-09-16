using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HomingProjectile : MonoBehaviour
{
	[SerializeField] private bool _hasTarget = false;
	[SerializeField] private float _speed;
	private List<GameObject> _enemiesList = new List<GameObject>();
	[SerializeField] private Transform _enemyTrans;

	[SerializeField] private Collider2D _detectCollider;
	[SerializeField] private Collider2D _detectEnemy;

	private void Start()
	{
		_hasTarget = false;
		_detectEnemy.enabled = false;
		transform.tag = "HomingProjectile";
	}

	private void Update()
	{
		if (_hasTarget == true)
		{
			float minDist = 0.1f;
			float dist = Vector3.Distance(transform.position, _enemyTrans.position);

			if (dist > minDist)
			{
				transform.position = Vector3.MoveTowards(transform.position, _enemyTrans.position, _speed * Time.deltaTime);
			}

			RotateTowards();
		}
		else
			Move();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if ((other.CompareTag("Enemy") || other.CompareTag("BigEnemy")) && !_enemiesList.Contains(other.gameObject))
		{
			_enemiesList.Add(other.gameObject);
			_enemyTrans = _enemiesList[0].transform;
			_hasTarget = true;

			StartCoroutine(DectedEnemyCoroutine());
		}
	}

	IEnumerator DectedEnemyCoroutine()
	{
		yield return new WaitForSeconds(0.5f);
		_detectCollider.enabled = false;
		_detectEnemy.enabled = true;
		transform.tag = "Laser";
	}

	private void Move()
	{
		transform.Translate(Vector3.up * _speed * Time.deltaTime);
	}

	private void RotateTowards()
	{
		Vector3 diff = _enemyTrans.position - transform.position;
		var offset = -90;

		//Mathf.Atan2 == Returns the angle in radians whose Tan(Returns the tangent of angle f in radians.) is y/x.
		float angle = Mathf.Atan2(diff.y, diff.x);

		// Mathf.Rad2Deg == Radians-to-degrees conversion constant.
		//This is equal to 360 / (PI * 2).
		transform.rotation = Quaternion.Euler(0f, 0f, (angle * Mathf.Rad2Deg) + offset);
	}

	private void OffScreen()
	{
		if (transform.position.y > 8f)
		{
			if (transform.parent != null)
			{
				Destroy(transform.parent.gameObject);
			}
			Destroy(gameObject);
		}
	}
}
