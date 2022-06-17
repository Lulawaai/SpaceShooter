using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
	private GameInputActions _input;

	[Header("ActionMaps")]
	[SerializeField] private Player _player;
	public static event Action OnFire;

	void Start()
	{
		_input = new GameInputActions();
		_input.Enable();
		_input.Player.Fire.performed += Fire_performed;
	}

	private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		OnFire?.Invoke();
	}

	private void Update()
	{
		movePlayer();
	}

	private void movePlayer()
	{
		Vector2 move;
		move = _input.Player.Move.ReadValue<Vector2>();

		if (_player != null)
		{
			_player.Move(move);
		}
	}
}
