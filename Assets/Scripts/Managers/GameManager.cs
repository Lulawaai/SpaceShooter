using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private bool _isGameOver;

	private void OnEnable()
	{
		Player.OnDeath += GameOver;
		GameInput.OnRestartGame += RestartGame;
		GameInput.OnQuitGame += QuitGame;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private void Start()
	{
		_isGameOver = false;
	}
	private void RestartGame()
	{
		if (_isGameOver == true)
		{
			SceneManager.LoadScene(0);
		}
	}

	private void GameOver()
    {
        _isGameOver = true;
    }

	private void OnDisable()
	{
		Player.OnDeath -= GameOver;
		GameInput.OnRestartGame -= RestartGame;
		GameInput.OnQuitGame -= QuitGame;
	}
}
