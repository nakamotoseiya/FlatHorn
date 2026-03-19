using UnityEngine;

public class ESC : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

	}

    // Update is called once per frame
    void Update()
    {
		// ESCキーでカーソルのロック解除/再ロック
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			
			if(Cursor.lockState == CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
	}
}

