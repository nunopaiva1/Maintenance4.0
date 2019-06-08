using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public Text nomeOperador, nomeTarefa, duracao, cont;
    string result = "";
    string date = "";
    string count = "";

    List<string> datalist = new List<string>();
    //Data current;
    private void Start()
    {
        nomeOperador.text = "Utilizador: " + SendNameDB.username;

        string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
        //This is my insert query in which i am taking input from the user through windows forms  
        //string Query = "SELECT Procedimento, Data  FROM catraport.Procedimento WHERE utilizador = '" + SendNameDB.username + "' and Procedimento is not NULL and Data is not NULL; ";
        string Query = "SELECT Procedimento, Data,COUNT(*) as count FROM catraport.Procedimento where utilizador = '" + SendNameDB.username + "' and Procedimento is not NULL and Data is not NULL GROUP BY Procedimento, Data ORDER BY count DESC;";
        //This is  MySqlConnection here i have created the object and pass my connection string.  
        MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
        //This is command class which will handle the query and connection object.  
        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
        MyConn2.Open();
        MySqlDataReader reader = MyCommand2.ExecuteReader();
        while (reader.Read())
        {
           // Debug.Log("inside while");
            //if (result != "" || date != "")
            //{
                result = Convert.ToString(reader[0]);
                date = Convert.ToString(reader[1]);
                count = Convert.ToString(reader[2]);
            //string date = reader.GetString(1);
            Debug.Log("proc = " + result);
            Debug.Log("date = " + date);
            Debug.Log("count = " + count);

            nomeTarefa.text = "Tarefa: " + result + "\n";
            duracao.text = "Data: " + date;
            cont.text = "Número de vezes realizado: " + count;
            //string[] procedu = { result, date };

            //datalist.AddRange(procedu);

            //foreach (string proc in datalist)
            //{
            //    nomeTarefa.text = "Tarefa: " + proc+"\n";
            //}
            //duracao.text = "Data: " + date;
            //}
        }
        //MyCommand2.ExecuteNonQuery();
        MyConn2.Close();

        
    }

    //static public Data Getproc(List<Data> datalist, string procedure, string data)
    //{
    //    for (int i = 0; i < datalist.Count; i++)
    //    {            
    //            return (datalist[i]);
    //    }
    //    return (null);
    //}

    //public class Data
    //{
    //    public string proc;
    //    public string dat;
        

    //    public Data(string proc, string dat)
    //    {
    //        this.proc = proc;
    //        this.dat = dat;
    //    }

    //}
}
