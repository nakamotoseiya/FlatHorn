using UnityEngine;

public class AppEntryPoint
{

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void EntryPoint()
	{
		Application.targetFrameRate = 60;
	}


}
