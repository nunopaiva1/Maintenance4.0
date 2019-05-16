using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Addbuttons : MonoBehaviour {

    public GameObject button;
    public GameObject content;
    private string Name;
    private int value;
    private int number;
    private string nam = "";
    // Use this for initialization
    void Start () {
        number = PlayerPrefs.GetInt("number");


        string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
        string Query = "select count(Name)" +
            " from catraport.Procedimentos;";            
        MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);  
        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
        MyConn2.Open();
        MySqlDataReader reader = MyCommand2.ExecuteReader();
        //int Value = MyCommand2.ExecuteNonQuery();
        while (reader.Read())
        {
            value = Convert.ToInt16(reader[0]);
            Debug.Log(value) ;
        }
        //MyCommand2.ExecuteNonQuery();
        MyConn2.Close();
        PlayerPrefs.SetInt("number", value);

        //if(value == 0)
        //{
        //    Debug.Log("Inside 0");
        //    return;
        //}
        //else if (value > 0)
        //{
        //    if(number == value)
        //    {
        //        Debug.Log("Inside =");
        //        return;
        //    }
        //    else if(number != value)
        //    {
        Debug.Log("Inside !=");
        string MyConnection1 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
        string Query1 = "select Name" +
            " from catraport.Procedimentos" +
            "ORDER BY ID DESC LIMIT 1;";
        MySqlConnection MyConn1 = new MySqlConnection(MyConnection1);
        MySqlCommand MyCommand1 = new MySqlCommand(Query1, MyConn1);
        MyConn1.Open();
        MySqlDataReader readr = MyCommand1.ExecuteReader();
        //int Value = MyCommand2.ExecuteNonQuery();
        while (readr.Read())
        {
            nam = Convert.ToString(readr[0]);
            Debug.Log("val = " + nam);
        }
        //MyCommand2.ExecuteNonQuery();
        MyConn1.Close();

        GameObject newButton = Instantiate(button) as GameObject;
        newButton.transform.SetParent(content.transform, false);
        //newButton.GetComponent < Text > = nam;
        //}
        //}


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
