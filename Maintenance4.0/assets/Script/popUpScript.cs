using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popUpScript : MonoBehaviour {


    public GameObject popUpPanel;

    public void setActive()
    {
        if (popUpPanel != null)
        {
            popUpPanel.SetActive(true);
        }
    }

    public void disableAgain()
    {
        if (popUpPanel != null)
        {
            popUpPanel.SetActive(false);
        }
    }

}
