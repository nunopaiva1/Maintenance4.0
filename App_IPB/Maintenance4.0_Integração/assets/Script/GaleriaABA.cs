using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GaleriaABA : MonoBehaviour {
    public GameObject PopUp;
    public RawImage image;
    public Texture[] gallery;
    public Texture Imagem;
    public Button Seguinte; //Button to view next image
    public Button Anterior; //Button to view previous image
    public VideoClip videoClip;
    private VideoPlayer videoPlayer;
    private int i = 0;

    public void BtnNext()
    {
        Destroy(videoPlayer);
        if (i < gallery.Length)
        {
            i++;

            if (i == gallery.Length) SceneManager.LoadScene("final");
        }
        //image.texture = Imagem;
        
    }

    public void BtnPrev()
    {
        if (i > 1)
        {
            i--;
        }
        else
        {
            StartCoroutine(Video());
        }
    }


    public void Sequence()
    {
        PopUp.SetActive(true);
        StartCoroutine(Video());        
    }

    public void CloseWindow()
    {
        Destroy(videoPlayer);
        PopUp.SetActive(false);

    }

    public IEnumerator Video()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.clip = videoClip;
        videoPlayer.Prepare();
        //wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitTime;
            //break out of the while loop after 5 seconds wait
            break;
        }
        //Assign the texture from video to rawimage to be displayed
        image.texture = videoPlayer.texture;
        //play video and sound
        videoPlayer.Play();
    }

    void Update()
    {
        image.texture = gallery[i];
    }

}
