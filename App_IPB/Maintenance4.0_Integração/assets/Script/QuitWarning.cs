using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;  //Its for MySQL

public class QuitWarning : MonoBehaviour {
    public CanvasGroup uiCanvasGroup;
    public CanvasGroup confirmQuitCanvasGroup;
    public GameObject destroy;


    // Use this for initialization
    private void Awake()
    {
        //disable the quit confirmation panel
        DoConfirmQuitNo();
    }

    /// <summary>
    /// Called if clicked on No (confirmation)
    /// </summary>
    public void DoConfirmQuitNo()
    {
        //enable the normal ui
        uiCanvasGroup.alpha = 1;
        uiCanvasGroup.interactable = true;
        uiCanvasGroup.blocksRaycasts = true;

        //disable the confirmation quit ui
        confirmQuitCanvasGroup.alpha = 0;
        confirmQuitCanvasGroup.interactable = false;
        confirmQuitCanvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Called if clicked on Yes (confirmation)
    /// </summary>
    public void DoConfirmQuitYes()
    {
        SceneManager.LoadScene("list", LoadSceneMode.Additive);
        string ID1 = "O procedimento foi cancelado antes de ter sido finalizado";
        try
        {
            string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
            //This is my insert query in which i am taking input from the user through windows forms  
            //string Query = "INSERT INTO catraport.Procedimento(utilizador, Id, Data, HoraInício, HoraFim, StepCount) VALUES('" + putsomething + "','" + ID1 + "','" + date + "','" + starttime + "','" + endtime + "','" + step + "');";
            string Query = "insert into catraport.Procedimento(Id) values('" + ID1 + "');";
            //This is  MySqlConnection here i have created the object and pass my connection string.  
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            //This is command class which will handle the query and connection object.  
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
            MyConn2.Open();
            MyCommand2.ExecuteNonQuery();
            MyConn2.Close();
        }
        catch
        {
            Debug.Log("Erro na base de dados");
        }
        Destroy(confirmQuitCanvasGroup);
        Destroy(destroy);
    }

    /// <summary>
    /// Called if clicked on Quit
    /// </summary>
    public void DoQuit()
    {
        //reduce the visibility of normal UI, and disable all interraction
        uiCanvasGroup.alpha = 0.5f;
        uiCanvasGroup.interactable = false;
        uiCanvasGroup.blocksRaycasts = false;

        //enable interraction with confirmation gui and make visible
        confirmQuitCanvasGroup.alpha = 1;
        confirmQuitCanvasGroup.interactable = true;
        confirmQuitCanvasGroup.blocksRaycasts = true;
    }  
}
