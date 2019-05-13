using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using MySql.Data.MySqlClient;  //Its for MySQL

public class SendNameDB : MonoBehaviour {

    static public string username;
    public GameObject panel_confirm;
    public Text name, titulo;

    // Use this for initialization
    void Start() {
    }
	
    public void Id(string st)
    {
        username = st;
        panel_confirm.SetActive(true);
        name.GetComponent<Text>().text = username;
        titulo.text = "Perfil selecionado, deseja continuar?";
        //database(st);
    }

    public void noClick()
    {
        panel_confirm.SetActive(false);
        titulo.text = "Selecione o seu perfil";
    }

    /*
	public void Id1() {
        string st = "Mário Fernandes";
        database(st);
    }

    public void Id2()
    {
        string st = "Carlos Teixeira";
        database(st);
    }
        public void Id3()
    {
        string st = "João Pedro Teixeira";
        database(st);
    }

    public void Id4()
    {
        string st = "Bruno Águeda";
        database(st);
    }

    public void Id5()
    {
        string st = "André Eiras";
        database(st);
    }

    public void Id6()
    {
        string st = "Henrique Dias Fim";
        database(st);
    }

    public void Id7()
    {
        string st = "João Sobrinho Teixeira";
        database(st);
    }

    public void Id8()
    {
        string st = "Ricardo Oliveira";
        database(st);
    }

    public void Id9()
    {
        string st = "Gonçalo Afonso";
        database(st);
    }

    public void Id10()
    {
        string st = "Nuno Garcia";
        database(st);
    }

    public void Id11()
    {
        string st = "Rui Gonçalves";
        database(st);
    }

    public void Id12()
    {
        string st = "Ruben Brito";
        database(st);
    }

    public void Id13()
    {
        string st = "Nuno Paulo";
        database(st);
    }

    public void Id14()
    {
        string st = "Sílvia Rodrigues";
        database(st);
    }

    public void Id15()
    {
        string st = "José Amaral";
        database(st);
    }

    public void Id16()
    {
        string st = "José Gonçalves";
        database(st);
    }

    public void Id17()
    {
        string st = "Cláudia Águeda";
        database(st);
    }

    public void Id18()
    {
        string st = "Diana Cidre";
        database(st);
    }

    public void Id19()
    {
        string st = "Ana Fidalgo";
        database(st);
    }

    public void Id20()
    {
        string st = "Sandra Marcelino";
        database(st);
    }

    public void Id21()
    {
        string st = "Adelaide Pinto";
        database(st);
    }

    public void Id22()
    {
        string st = "Vitor Campos";
        database(st);
    }

    public void Id23()
    {
        string st = "Manuela Silva";
        database(st);
    }

    public void Id24()
    {
        string st = "Daniela Domingues";
        database(st);
    }

    public void Id25()
    {
        string st = "Vanessa Cordeiro";
        database(st);
    }

    public void Id26()
    {
        string st = "Susete Almeida";
        database(st);
    }

    public void Id27()
    {
        string st = "Marlena Gomes";
        database(st);
    }

    public void Id28()
    {
        string st = "Pedro Pipa";
        database(st);
    }

    public void Id29()
    {
        string st = "Tiago Rodrigo";
        database(st);
    }

    public void Id30()
    {
        string st = "Valter Apolinário";
        database(st);
    }

    public void Id31()
    {
        string st = "Tiago Pardelinha";
        database(st);
    }

    public void Id32()
    {
        string st = "Ana Isabel Fernandes";
        database(st);
    }

    public void Id33()
    {
        string st = "David Batista";
        database(st);
    }

    public void Id34()
    {
        string st = "Elisabete Rodrigues";
        database(st);
    }

    public void Id35()
    {
        string st = "Francisco Bessa";
        database(st);
    }

    public void Id36()
    {
        string st = "José Antas";
        database(st);
    }

    public void Id37()
    {
        string st = "Tânia Brás";
        database(st);
    }

    public void Id38()
    {
        string st = "Zélia Fernandes";
        database(st);
    }

    public void Id39()
    {
        string st = "Paula Remondes";
        database(st);
    }

    public void Id40()
    {
        string st = "Dália Teixeira";
        database(st);
    }
    */

   /* public void database(string st)
    {
        try
        {
            //This is my connection string i have assigned the database file address path  
            string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
            //This is my insert query in which i am taking input from the user through windows forms  
            //string Query = "INSERT INTO catraport.Procedimento(utilizador, Id, Data, HoraInício, HoraFim, StepCount) VALUES('" + putsomething + "','" + ID1 + "','" + date + "','" + starttime + "','" + endtime + "','" + step + "');";
            string Query = "insert into catraport.Procedimento(utilizador) values('" + st + "');";
            //This is  MySqlConnection here i have created the object and pass my connection string.  
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            //This is command class which will handle the query and connection object.  
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
            MyConn2.Open();
            MyCommand2.ExecuteNonQuery();
            MyConn2.Close();
            PlayerPrefs.SetString("name", st);
        }
        catch
        {
            Debug.Log("Erro na base de dados");
        }
    }
    */

}
