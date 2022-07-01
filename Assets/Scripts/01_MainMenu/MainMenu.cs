using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //this allows us to 

public class MainMenu : MonoBehaviour
{
	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}
}
