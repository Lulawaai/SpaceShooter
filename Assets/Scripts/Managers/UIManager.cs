using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	[SerializeField] private TMP_Text _scoreText;
	[SerializeField] private TMP_Text _waveText;
	[SerializeField] private Sprite[] _lives;
	[SerializeField] private Image _livesImage;
	[SerializeField] private float _timeGameOverFlicker;
	[SerializeField] private bool _mobileDevice = true;
	private int _waveNr;

	[SerializeField] private GameObject[] _laserAvailable;
    [SerializeField] private int _nrLaser = 14;

	[Header("ButtonsUI")]
	[SerializeField] private TMP_Text _gameOverText;
	[SerializeField] private TMP_Text _restartText;
	[SerializeField] private GameObject _restartButton;
	[SerializeField] private GameObject _joyStick;
	[SerializeField] private GameObject _fireOnscreenButton;
	[SerializeField] private GameObject _mobileOption;
	[SerializeField] private GameObject _desktopOption;
	[SerializeField] private GameObject _PlayerWonLeveltxt;

	private WaitForSeconds _waitFlicker;

	private bool _isPlayerAlive = true;

    private int _score;

	private void OnEnable()
	{
		Enemy.OnEnemyDeathLaser += UpdateScore;
		BigEnemy.OnEnemyDeadScore += UpdateScore;
		EnemyBossAI.OnBossAIHit += UpdateScore;
		EnemyBossAI.OnBossAIDeath += PlayerWonLevel;
		Player.OnLossingLives += UpdateLives;
        Player.OnDeath += PlayerDeath;
		Player.OnPlayerFiring += FireRemoving;
        PowerUp.OnPlayerHit_FireRefill += FireAdding;
        SpawnManager.OnUpdatingWaveNr += UpdateWaveNr;
	}

	void Start()
	{
		_mobileOption.SetActive(true);
		_desktopOption.SetActive(false);

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

	private void UpdateWaveNr(int waveNr)
    {
        _waveText.text = "Wave nr: " + waveNr.ToString();
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

		if (_mobileDevice == true)
		{
			_joyStick.SetActive(true);
			_fireOnscreenButton.SetActive(true);
		}
		else
		{
			_joyStick.SetActive(false);
			_fireOnscreenButton.SetActive(false);
		}
	}

	private void FireRemoving(int nrFire)
    {
        _nrLaser = nrFire;
		if (_nrLaser > -1 )
        {
            _laserAvailable[_nrLaser].SetActive(false);
        }
    }

    private void FireAdding()
    {
		int i = 0;
        while (i < 15)
        {
			_laserAvailable[i].SetActive(true);
			i++;
        }
    }

    private void GameOverSteps()
	{
		_restartText.gameObject.SetActive(true);
		_restartButton.SetActive(true);
		_joyStick.SetActive(false);
		_fireOnscreenButton.SetActive(false);
	}

	private void PlayerWonLevel()
	{
		_PlayerWonLeveltxt.SetActive(true);
		_restartText.gameObject.SetActive(true);
		_restartButton.SetActive(true);
		_joyStick.SetActive(false);
		_fireOnscreenButton.SetActive(false);
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

	public void MobileDevice()
	{
		_mobileDevice = true;
		GameStartSteps();
		_mobileOption.SetActive(false);
		_desktopOption.SetActive(true);
		_fireOnscreenButton.SetActive(true);
		_joyStick.SetActive(true);
	}

	public void DesktopDevice()
	{
		_mobileDevice = false;
		GameStartSteps();
		_mobileOption.SetActive(true);
		_desktopOption.SetActive(false);
		_fireOnscreenButton.SetActive(false);
		_joyStick.SetActive(false);
	}

	private void OnDisable()
	{
		Enemy.OnEnemyDeathLaser -= UpdateScore;
		Player.OnLossingLives -= UpdateLives;
		Player.OnDeath -= PlayerDeath;
        Player.OnPlayerFiring -= FireRemoving;
        PowerUp.OnPlayerHit_FireRefill -= FireAdding;
		SpawnManager.OnUpdatingWaveNr -= UpdateWaveNr;
		BigEnemy.OnEnemyDeadScore -= UpdateScore;
		EnemyBossAI.OnBossAIHit -= UpdateScore;
		EnemyBossAI.OnBossAIDeath -= PlayerWonLevel;
	}
}
