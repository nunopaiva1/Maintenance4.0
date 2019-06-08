using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Addbuttons : MonoBehaviour {

    public delegate void ClickAction();
    public static event ClickAction OnClicked;
    private GameObject buttt;

    GetButton get;

    public GameObject button;
    public GameObject content;
    private string Name;
    private int value;
    private int number;
    private string nam = "";
    private string xml = "";
    private string click = "";
    // Use this for initialization
    void Start () {
        string MyConnection1 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
        string Query1 = "select Name, Xml" +
            " from catraport.Procedimentos;";
        MySqlConnection MyConn1 = new MySqlConnection(MyConnection1);
        MySqlCommand MyCommand1 = new MySqlCommand(Query1, MyConn1);
        MyConn1.Open();
        MySqlDataReader readr = MyCommand1.ExecuteReader();
        //int Value = MyCommand2.ExecuteNonQuery();
        while (readr.Read())
        {
            nam = Convert.ToString(readr[0]);
            xml = Convert.ToString(readr[1]);
            Debug.Log("val = " + nam);

            GameObject newButton = Instantiate(button) as GameObject;
            newButton.transform.SetParent(content.transform, false);
            Button buttt = newButton.GetComponent<Button>();
            buttt.onClick.AddListener(() => clickAction(buttt));
            buttt.onClick.AddListener(Change);           
            buttt.transform.GetChild(0).GetComponent<Text>().text = nam;
        }
        MyConn1.Close();
    }
    void clickAction(Button buttonClicked)
    {
        get = GetComponent<GetButton>();
        get.onDisable();
    }
    void Change()
    {
        SceneManager.LoadScene("Procedimentos");
    }
}
