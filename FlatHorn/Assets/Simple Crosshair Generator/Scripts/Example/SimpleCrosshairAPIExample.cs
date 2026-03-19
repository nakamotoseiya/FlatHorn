using UnityEngine;

public class SimpleCrosshairAPIExample : MonoBehaviour
{
    public SimpleCrosshair simpleCrosshair;

    private void Start()
    {
        if(simpleCrosshair == null)
        {
            Debug.LogError("You have not set the target SimpleCrosshair. Disabling the example script.");
            enabled = false;
        }
    }

    
}
