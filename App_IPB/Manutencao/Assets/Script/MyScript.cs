using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScript : MonoBehaviour {
    public GameObject button;
    public GameObject content;

    // Use this for initialization
    public void BuildObject ()
    {
        //GameObject instance = Instantiate(Resources.Load("Button", typeof(GameObject))) as GameObject;
        GameObject newButton = Instantiate(button) as GameObject;
        newButton.transform.SetParent(content.transform, false);
    }
	
	
}
