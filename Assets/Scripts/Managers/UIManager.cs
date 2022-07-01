using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TMP_Text _scoreText;
	[SerializeField] private Sprite[] _lives;
	[SerializeField] private Image _livesImage;
	[SerializeField] private float _timeGameOverFlicker;

	[Header("TextUI")]
	[SerializeField] private TMP_Text _gameOverText;
	[SerializeField] private TMP_Text _restartText;
	[SerializeField] private GameObject _restartButton;

	private WaitForSeconds _waitFlicker;

	private bool _isPlayerAlive = true;

    private int _score;

	private void OnEnable()
	{
		Enemy.OnEnemyDeath += UpdateScore;
		Player.OnLossingLives += UpdateLives;
		Player.OnDeath += PlayerDeath;
	}

	void Start()
	{
		GameStartSteps();

		_scoreText.text = "Score: " + _score.ToString();
		_livesImage.sprite = _lives[3];
		_isPlayerAlive = true;
		_waitFlicker = new WaitForSeconds(_timeGameOverFlicker);
	}

	private void UpdateScore(int score)
	{
		_score += score;
		_scoreText.text = "Score: " + _score.ToString();
	}

	private void UpdateLives(int lives)
	{
		switch(lives)
		{
			case 0:
				_livesImage.sprite = _lives[0];
				break;
			case 1:
				_livesImage.sprite = _lives[1];
				break;
			case 2:
				_livesImage.sprite = _lives[2];
				break;
			case 3:
				_livesImage.sprite = _lives[3];
				break;
		}
	}

	private void PlayerDeath()
	{
		_isPlayerAlive = false;

		StartCoroutine(GameoverFlickerRoutine());
		GameOverSteps();
	}

	private void GameStartSteps()
	{
		_gameOverText.gameObject.SetActive(false);
		_restartText.gameObject.SetActive(false);
		_restartButton.SetActive(false);
	}

	private void GameOverSteps()
	{
		_restartText.gameObject.SetActive(true);
		_restartButton.SetActive(true);
	}

	IEnumerator GameoverFlickerRoutine()
	{
		while (_isPlayerAlive == false)
		{
			_gameOverText.gameObject.SetActive(true);

			yield return _waitFlicker;
			_gameOverText.gameObject.SetActive(false);

			yield return _waitFlicker;
		}

	}

	private void OnDisable()
	{
		Enemy.OnEnemyDeath -= UpdateScore;
		Player.OnLossingLives -= UpdateLives;
		Player.OnDeath -= PlayerDeath;
	}
}
