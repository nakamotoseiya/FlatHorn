using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
	public void OnStartButton()
	{
		SceneManager.LoadScene("Game Scene");

	}
	public void OnEndButton()
	{
		UnityEditor.EditorApplication.isPlaying = false;
		//Application.Quit();
	}
}
