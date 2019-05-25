using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public void ChangeScene(string sceaneName)
    {
        Application.LoadLevel(sceaneName);
    }

}
