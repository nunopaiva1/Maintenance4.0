using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using MySql.Data.MySqlClient;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GetButton : MonoBehaviour {

    public Button Aba;
    public Button MesasM;
    public Button Prensas;
    public Button TTM;
    public Button ATPS;
    public Button TTA;
    public Button TCA;
    public Button TCM;
    public Button TTPM;

    string click = "";

    // Use this for initialization
    void Start () {
    }
    public void onDisable()
    {
        click = EventSystem.current.currentSelectedGameObject.name;
        PlayerPrefs.SetString("filename", click+".xml");
    }
    // Update is called once per frame
    void Update () {
		
	}
}
