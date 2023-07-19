using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawner : MonoBehaviour
{
    private CameraController _cameraController;
    void Start()
    {
        GameObject cameraObject = new("Main Camera");

        cameraObject.AddComponent<CameraController>();

        cameraObject.transform.position = new Vector3(0f, 0f, -10f); // needs to track the player down

        // Set the camera as the main camera in the scene manually
        Camera previousMainCamera = Camera.main;
        if (previousMainCamera != null)
        {
            previousMainCamera.tag = "Untagged";
        }
        cameraObject.tag = "MainCamera";

        // Optionally, you can destroy the previous main camera GameObject
        if (previousMainCamera != null)
        {
            Destroy(previousMainCamera.gameObject);
        }
    }
    public void InitializeCameraSpawner(CameraController cameraController)
    {
        _cameraController = cameraController;
    }
}
