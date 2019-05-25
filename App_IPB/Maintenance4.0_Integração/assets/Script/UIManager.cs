using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image img;
    public Text nome;

    void Start()
    {
        img.GetComponent<Image>().sprite = buttonClickLogin.img;
        nome.text = SendNameDB.username;
    }

}
