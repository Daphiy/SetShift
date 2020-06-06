using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public bool remoteLogger = true;
    void Start()
    {
        if (remoteLogger)
            RemoteLogger.Enable();
        else
            RemoteLogger.Disable();

       // Debug.Log("Test - Log");
        //Debug.LogWarning("Test - Warning");
        //Debug.LogError("Test - Error");
    }
}
