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
	public static event Action OnRestartGame;
	public static event Action OnQuitGame;
	public static event Action OnSpeedingUP;
	public static event Action OnSetBackNormalSpeed;
	public static event Action OnCollectPickUPs;
	public static event Action OnCollectPickUpsFinished;

	void Start()
	{
		_input = new GameInputActions();
		_input.Enable();
		_input.Player.Fire.performed += Fire_performed;
		_input.Player.RestartGame.performed += RestartGame_performed;
		_input.Player.ESCGame.performed += ESCGame_performed;
	}

	private void ESCGame_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		OnQuitGame?.Invoke();
	}

	private void RestartGame_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		OnRestartGame?.Invoke();
	}

	private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		OnFire?.Invoke();
	}

	private void Update()
	{
		MovePlayer();
		SpeedUP();
		BackNormalSpeed();

		MovePickUpTowarsPlayer();
		StopMovingPickUpTowardsPlayer();
	}

	private void MovePickUpTowarsPlayer()
	{
		if (_input.Player.CollectPickUps.IsPressed())
		{
			OnCollectPickUPs?.Invoke(); //Player to send her Pos to PowerUps
		}
	}

	private void StopMovingPickUpTowardsPlayer()
	{
		if (_input.Player.CollectPickUps.WasReleasedThisFrame())
		{
			OnCollectPickUpsFinished?.Invoke(); //PowerUps to move back down
		}
	}

	private void MovePlayer()
	{
		Vector2 move;
		move = _input.Player.Move.ReadValue<Vector2>();

		if (_player != null)
		{
            _player.Move(move);
		}
	}

	private void SpeedUP()
    {
		if (_input.Player.SpeedUP.IsPressed())
		{
			OnSpeedingUP?.Invoke();
		}
	}

	private void BackNormalSpeed()
    {
		if (_input.Player.SpeedUP.WasReleasedThisFrame())
		{
			OnSetBackNormalSpeed?.Invoke();
		}
	}
}
