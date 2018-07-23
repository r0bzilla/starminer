using UnityEngine;

public abstract class AbstractShipEngine : MonoBehaviour
{
    public bool isRunning { get; protected set; }

    public float forwardSpeed { get; protected set; }

    public float turnSpeed { get; protected set; }

    protected abstract void OnEngineStarted();

    public abstract void OnEngineRunning(float horizontalInput, float verticalInput);

    protected abstract void OnEngineStopped();


    public void StartEngine()
    {
        if (!isRunning)
        {
            isRunning = true;
            OnEngineStarted();
        }
    }


    public void StopEngine()
    {
        if (isRunning)
        {
            isRunning = false;
            OnEngineStopped();
        }
    }
}