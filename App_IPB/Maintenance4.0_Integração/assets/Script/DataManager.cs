using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public Text nomeOperador, nomeTarefa, duracao;

    private void Start()
    {
        nomeOperador.text = "Utilizador: " + SendNameDB.username;
        nomeTarefa.text = "sem dados";
        duracao.text = "sem dados";

        // ir buscar à base de dados os três atributos
        // atribuir nomeOperador.text e restantes caso exista histórico para ser mostrado 
    }
}
