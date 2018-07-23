using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StarMiner.Context.Space.Ship.Engine;


namespace StarMiner.Context.Space.Ship
{
    public class ShipController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D ship;

        [SerializeField]
        private float driveAcceleration;

        [SerializeField]
        private float driveDeceleration;

        [SerializeField]
        private float maxDriveSpeed;

        [SerializeField]
        private float turnAcceleration;

        [SerializeField]
        private float turnDeceleration;

        [SerializeField]
        private float maxTurnSpeed;

        private float currentTurnSpeed = 0;

        [SerializeField]
        private List<AbstractShipEngine> engines;

        // Use this for initialization
        protected void Awake()
        {
        }

        private Vector2 force = new Vector2();

        protected void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");


            // set ship rotation
            currentTurnSpeed = CalculateCurrentTurnSpeed(horizontalInput) * Time.deltaTime;
            //ship.rotation += currentTurnSpeed;
            ship.transform.Rotate(0, 0, currentTurnSpeed);

            // TODO only run once
            GetComponentsInChildren<AbstractShipEngine>(engines);

            float forwardSpeed = 0f;
            if (verticalInput == 0f)
            {
                foreach (AbstractShipEngine e in engines)
                    e.StopEngine();
            }
            else
            {
                foreach (AbstractShipEngine e in engines)
                {
                    e.StartEngine();
                    e.OnEngineRunning(horizontalInput, verticalInput);
                    forwardSpeed += e.forwardSpeed;
                }
            }

            float rotaZ = (ship.transform.rotation.eulerAngles.z) * Mathf.Deg2Rad;
            force.x = -Mathf.Sin(rotaZ);
            force.y = Mathf.Cos(rotaZ);
            force *= forwardSpeed * Time.deltaTime;

            ship.AddForce(force);
        }


        private float CalculateCurrentTurnSpeed(float horizontalInput)
        {
            float oldTurnSpeed = currentTurnSpeed;
            float newTurnSpeed = oldTurnSpeed + horizontalInput * turnAcceleration;

            newTurnSpeed -= Mathf.Sign(newTurnSpeed) * Mathf.Min(Mathf.Abs(newTurnSpeed), turnDeceleration);

            if (horizontalInput == 0f && Mathf.Abs(newTurnSpeed) <= turnDeceleration)
                newTurnSpeed = 0f;

            if (oldTurnSpeed <= maxTurnSpeed && newTurnSpeed > maxTurnSpeed)
                newTurnSpeed = maxTurnSpeed;

            else if (oldTurnSpeed >= -maxTurnSpeed && newTurnSpeed < -maxTurnSpeed)
                newTurnSpeed = -maxTurnSpeed;

            return newTurnSpeed;
        }
    }
}
