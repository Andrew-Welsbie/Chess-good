using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winScreen : MonoBehaviour
{

   

    public GameObject winMenuUI;

    // Update is called once per frame
   
    
    public void win()
    {
        winMenuUI.SetActive(true);

        GameObject varGameObject = GameObject.Find("Board");
        varGameObject.GetComponent<TileSelector>().enabled = false;
        
       
    }
   
}
