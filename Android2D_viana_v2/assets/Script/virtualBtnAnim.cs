using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class virtualBtnAnim : MonoBehaviour, IVirtualButtonEventHandler {

    public GameObject vbBtnObj, mark, cube;
    public DialogueManager dManager;

    // Use this for initialization
    void Start()
    {

        vbBtnObj.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        vbBtnObj = GameObject.Find("botaoVirtual");

       /* mark = GameObject.Find("mark");
        mark.SetActive(true);

        cube = GameObject.Find("step1");
        cube.SetActive(false);*/
    }

    public void OnButtonPressed(VirtualButtonBehaviour vbBtnObj)
    {
       /* mark.SetActive(false);
        cube.SetActive(true);
        //Destroy(vbBtnObj);*/

        dManager.DisplayNextSentence();
    }

    public void OnButtonReleased(VirtualButtonBehaviour vbBtnObj)
    {
        Debug.Log("Button released");
    }
}
