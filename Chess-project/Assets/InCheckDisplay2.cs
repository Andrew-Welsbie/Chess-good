using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InCheckDisplay2 : MonoBehaviour
{
    public GameObject CheckMenu;
    // Update is called once per frame
    void Update()
    {
        bool InCheck = GameManager.instance.check();
        bool OtherInCheck = GameManager.instance.OtherKingInCheck();
        if (InCheck || OtherInCheck)
        {
            
            CheckMenu.SetActive(true);
        }
        else
        {
            CheckMenu.SetActive(false);
        }

    }
}
