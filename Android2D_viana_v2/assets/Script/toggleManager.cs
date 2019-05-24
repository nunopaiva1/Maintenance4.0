using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class toggleManager : MonoBehaviour {

    public Camera AR_cam, main_cam;
    public GameObject video;

    // Use this for initialization
    void Start()
    {
        Toggle toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(toggle);
        });
    }

    private void ToggleValueChanged(Toggle toggle)
    {
        AR_cam.enabled = !AR_cam.enabled;
        main_cam.enabled = !main_cam.enabled;

        if (main_cam.enabled)
        {
            video.SetActive(true);
            Debug.Log("video reproduziu");
        }
        else
        {
            video.SetActive(false);
            Debug.Log("video está desligado");
        }
    }
}
