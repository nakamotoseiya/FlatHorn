using UnityEngine;

public class PanelController : MonoBehaviour
{
	public GameObject panel;

	void Start()
	{
		panel.SetActive(false);
	}

	public void OpenPanel()
	{
		panel.SetActive(true);
	}

	public void ClosePanel()
	{
		panel.SetActive(false);
	}
}