using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;


namespace StarMiner.Context.Space.Ship
{
    public class ShipCamera : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D ship;

        [SerializeField]
        private Camera shipCamera;


        [SerializeField]
        private float minPaddingFactor = 1f;

        [SerializeField]
        private float maxZoomFactor = 3f;

        [SerializeField]
        private float zoomMaxGrace;

        private float zoomCurrentGrace;

        private float defaultZoom;
        private Vector3 shipCameraPosition;


        // Use this for initialization
        protected void Awake()
        {
            shipCameraPosition = new Vector3();
            shipCameraPosition.x = ship.position.x;
            shipCameraPosition.y = ship.position.y;
            shipCameraPosition.z = shipCamera.transform.position.z;

            shipCamera.transform.rotation = ship.transform.rotation;
            defaultZoom = CalculateDefaultZoom();
            shipCamera.orthographicSize = defaultZoom;
        }


        private float CalculateDefaultZoom()
        {
            // calculate the number of ship tiles in both directions
            Tilemap shipTileMap = ship.GetComponent<Tilemap>();
            Vector3Int shipTilesSize = shipTileMap.cellBounds.size;

            // calculate how many tiles can be displayed by the camera
            float cameraHeight = shipCamera.orthographicSize * 2;
            float cameraWidth = cameraHeight * shipCamera.aspect;

            //Debug.Log("Ship: " + shipTilesSize.x + ", " + shipTilesSize.y);
            //Debug.Log("Camera: " + cameraWidth + ", " + cameraHeight);
            //Debug.Log("Result: " + ((shipTilesSize.y / 2f) * (1f + minPaddingFactor)) + " or " + ((shipTilesSize.x / 2f / shipCamera.aspect) * (1f + minPaddingFactor)));

            // calculate the new camera zoom with ship size dependent padding
            return Mathf.Max(
                (shipTilesSize.y / 2f) * (1f + minPaddingFactor),
                (shipTilesSize.x / 2f / shipCamera.aspect) * (1f + minPaddingFactor)
            );
        }


        protected void Update()
        {
            //float horizontalInput = Input.GetAxis("Horizontal");
            //float verticalInput = Input.GetAxis("Vertical");

            // set camera
            shipCameraPosition.Set(
                Mathf.Lerp(shipCamera.transform.position.x, ship.position.x, 1.00f),
                Mathf.Lerp(shipCamera.transform.position.y, ship.position.y, 1.00f),
                shipCamera.transform.position.z
            );
            shipCamera.transform.position = shipCameraPosition;
            shipCamera.transform.rotation = ship.transform.rotation;
            shipCamera.orthographicSize = CalculateCameraZoom();
        }


        private float CalculateCameraZoom()
        {
            float oldZoom = shipCamera.orthographicSize;

            // calculate speed-dependent zoom
            float shipSpeed = ship.velocity.magnitude;
            float newZoom;
            newZoom = Mathf.Min(
                defaultZoom * maxZoomFactor,
                defaultZoom / 2 + shipSpeed / 2
            );
            newZoom = Mathf.Max(defaultZoom, newZoom);

            if (newZoom < oldZoom)
            {
                // if we decelerate, wait a bit before zooming in again
                if (zoomCurrentGrace > 0f)
                {
                    zoomCurrentGrace -= Time.deltaTime;
                    return oldZoom;
                }
            }
            else if (newZoom > oldZoom || newZoom == defaultZoom)
                zoomCurrentGrace = zoomMaxGrace;

            return Mathf.Lerp(oldZoom, newZoom, 0.1f);
        }
    }
}
