using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelingDriveShipEngine : AbstractShipEngine
{
    [SerializeField]
    private float driveAcceleration;

    [SerializeField]
    private float driveDeceleration;

    [SerializeField]
    private float maxDriveSpeed;

    [SerializeField]
    private AudioSource driveSound;

    [SerializeField]
    private ParticleSystem particles;

    protected override void OnEngineStarted()
    {
        driveSound.Play();
        driveSound.pitch = 0;
        driveSound.volume = 0.2f;
        particles.Play();
    }

    public override void OnEngineRunning(float horizontalInput, float verticalInput)
    {
        forwardSpeed = CalculateCurrentDriveSpeed(verticalInput);
        driveSound.pitch = Mathf.Abs(verticalInput);
        driveSound.volume = Mathf.Min(1f, Mathf.Abs(verticalInput) + 0.2f);
    }

    protected override void OnEngineStopped()
    {
        driveSound.Stop();
        forwardSpeed = 0f;
        particles.Stop();
    }

    private float CalculateCurrentDriveSpeed(float verticalInput)
    {
        float oldSpeed = forwardSpeed;
        float newSpeed = oldSpeed + verticalInput * driveAcceleration;

        newSpeed -= Mathf.Sign(newSpeed) * Mathf.Min(Mathf.Abs(newSpeed), driveDeceleration);

        if (verticalInput == 0f && Mathf.Abs(newSpeed) <= driveDeceleration)
            newSpeed = 0f;

        if (oldSpeed <= maxDriveSpeed && newSpeed > maxDriveSpeed)
            newSpeed = maxDriveSpeed;

        else if (oldSpeed >= -maxDriveSpeed && newSpeed < -maxDriveSpeed)
            newSpeed = -maxDriveSpeed;

        return newSpeed;
    }
}
