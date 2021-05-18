/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlacement : MonoBehaviour
{
    public static bool CamPlace = false;
    public GameObject setCamera;
    public GameObject mainCam;
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("in key down R statement");
            if (CamPlace)
            {
                Replace();
                Debug.Log("in IF statement");
            }
            else
            {
                Stay();
                Debug.Log("in ELSE statement");
            }
        }
    }
    public void Replace()
    {
        Debug.Log("HEllo");
        float positionY = GameObject.Find("FP camera").transform.position.y;
        float positionX = GameObject.Find("FP camera").transform.position.x;
        float positionZ = GameObject.Find("FP camera").transform.position.z;
        GameObject.Find("SetCam").transform.position = new Vector3(positionX, positionY, positionZ);
        mainCam.SetActive(false);
        setCamera.SetActive(true);
        CamPlace = false;
    }

    

    public void Stay()
    {
        setCamera.SetActive(false);
        CamPlace = true;
    }


}
*/