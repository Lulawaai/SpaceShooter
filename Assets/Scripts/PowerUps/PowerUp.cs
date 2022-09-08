using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUp : MonoBehaviour
{
	[SerializeField] private float _speed = 3.0f;
	[SerializeField] private bool _moveTowardsPlayer;
	[SerializeField] private Transform _playerTrans;
	[SerializeField] private float _speedTowardsPlayer;

	[Header("5-ExtraFire, 6-Slow")]
	[Header("0-TripleLaser, 1-Speed, 2-Shield, 3-Fire, 4-Health")]
	[SerializeField] int _powerUpID;

    public static event Action OnPlayerHit_TripleLaser;
    public static event Action OnPlayerHit_Speed;
    public static event Action OnPlayerHit_Shield;
	public static event Action OnPlayerHit_FireRefill;
    public static event Action OnPlayerHit_Health;
    public static event Action OnPlayerHit_ExtraFire;
	public static event Action OnPlayerHit_Slow;

	private void OnEnable()
	{
		Player.OnSharingPlayerPos += MoveTowardsPlayer;
		GameInput.OnCollectPickUpsFinished += StopMovingTowardsPlayer;
	}

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
				case 3:
					OnPlayerHit_FireRefill?.Invoke();
                    break;
                case 4:
                    OnPlayerHit_Health?.Invoke(); //Player && UIManager
                    break;
                case 5:
                    OnPlayerHit_ExtraFire?.Invoke();  //Player && UIManager
                    break;
				case 6:
					OnPlayerHit_Slow?.Invoke();  //Player
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
		if (_moveTowardsPlayer == false)
		{
			transform.Translate(Vector3.down * _speed * Time.deltaTime);
		}
		else if (_moveTowardsPlayer == true)
		{
			float withinRange = 0.1f;
			float dist = Vector3.Distance(transform.position, _playerTrans.position);

			if (dist > withinRange)
			{
				transform.position = Vector3.MoveTowards(transform.position, _playerTrans.position, _speedTowardsPlayer * Time.deltaTime);
			}
		}

		if (transform.position.y < -6.0f)
		{
			Destroy(this.gameObject);
		}
	}

	private void MoveTowardsPlayer (Transform player)
	{
		_playerTrans = player;
		_moveTowardsPlayer = true;
	}

	private void StopMovingTowardsPlayer()
	{
		_moveTowardsPlayer = false;
	}

	private void OnDisable()
	{
		Player.OnSharingPlayerPos -= MoveTowardsPlayer;
		GameInput.OnCollectPickUpsFinished -= StopMovingTowardsPlayer;
	}
}
