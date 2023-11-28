using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public void ChangeScene(string sceneName)
	{
		PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
		SceneManager.LoadScene(sceneName);
	}

	public void ReturnToLastScene()
	{
		SceneManager.LoadScene(PlayerPrefs.GetString("LastScene"));
		PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
	}
}
