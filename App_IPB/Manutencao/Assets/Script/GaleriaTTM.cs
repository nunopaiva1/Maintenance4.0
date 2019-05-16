using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GaleriaTTM : MonoBehaviour {

    public GameObject PopUp;
    public Image displayImage;
    public Sprite[] gallery;
    public Button Seguinte; //Button to view next image
    public Button Anterior; //Button to view previous image
    private int i = 0;

    public void BtnNext()
    {
        if(i  < gallery.Length)
        {
            i++;
        }
    }

    public void BtnPrev()
    {
        if (i > 0)
        {
            i--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        displayImage.sprite = gallery[i];
    }

    public void Sequence()
    {
        PopUp.SetActive(true);
    
    }

    public void CloseWindow()
    {
        PopUp.SetActive(false);

    }

    
}
