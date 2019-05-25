using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonClickLogin : MonoBehaviour
{

    static public Sprite img;
    public Image img2;

    public void click()
    {
        img2.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        img = GetComponent<Image>().sprite;
    }
}
