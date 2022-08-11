using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[SerializeField] private AudioSource _backgroundMusicAudioS;
    [SerializeField] private AudioSource _laserAudioS;
	[SerializeField] private AudioSource _explosionAudioS;
	[SerializeField] private AudioSource _powerUpAudioS;
	[SerializeField] private AudioSource _noMoreFireAudioS;

	private void OnEnable()
	{
		Player.OnPlayerFiring += PlayLaserSound;
		Player.OnExplosion += PlayExplosionSound;
		Player.OnPowerUp += PlayPowerUpSound;
		Player.OnDeath += StopBackgroundMusic;
		Player.OnOutofShots += PlayNoMoreAmmo;
		Asteroid.OnAsteroidDestroyed += PlayExplosionSound;
		Asteroid.OnAsteroidDestroyed += PlayBackgroundMusic;
		Enemy.OnEnemyDeathPlaySound += PlayExplosionSound;
	}

	private void PlayNoMoreAmmo()
    {
        _noMoreFireAudioS.Play();
    }

	private void PlayBackgroundMusic()
	{
		_backgroundMusicAudioS.Play();
	}

	private void StopBackgroundMusic()
	{
		_backgroundMusicAudioS.Stop();
	}

	private void PlayLaserSound(int nrLaser)
	{
		_laserAudioS.Play();
	}

	private void PlayExplosionSound()
	{
		_explosionAudioS.Play();
	}

	private void PlayPowerUpSound()
	{
		_powerUpAudioS.Play();
	}

	private void OnDisable()
	{
		Player.OnPlayerFiring -= PlayLaserSound;
		Player.OnExplosion -= PlayExplosionSound;
		Player.OnPowerUp -= PlayPowerUpSound;
		Player.OnDeath -= StopBackgroundMusic;
		Player.OnOutofShots -= PlayNoMoreAmmo;
		Asteroid.OnAsteroidDestroyed -= PlayExplosionSound;
		Asteroid.OnAsteroidDestroyed -= PlayBackgroundMusic;
		Enemy.OnEnemyDeathPlaySound -= PlayExplosionSound;
	}
}
