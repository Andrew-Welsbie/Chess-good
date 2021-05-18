using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public static bool CamPlace = false;
    public Camera firstPersonCamera;
    public Camera overheadCamera;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("in key down R statement");
            if (CamPlace)
            {
                ShowFirstPersonView();
            }
            else
            {
                ShowOverheadView();
            }
        }


    }


    // Call this function to disable FPS camera,
    // and enable overhead camera.
    public void ShowOverheadView()
    {
        firstPersonCamera.enabled = false;
        overheadCamera.enabled = true;
        CamPlace = true;
    }

    // Call this function to enable FPS camera,
    // and disable overhead camera.
    public void ShowFirstPersonView()
    {
        firstPersonCamera.enabled = true;
        overheadCamera.enabled = false;
        CamPlace = false;
    }
}
