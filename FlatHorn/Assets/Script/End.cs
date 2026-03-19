using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
	public void OnTitleButton()
	{
		SceneManager.LoadScene("Title Scene");

	}
	public void OnEndButton()
	{
		UnityEditor.EditorApplication.isPlaying = false;
		//Application.Quit();
	}
}

	
