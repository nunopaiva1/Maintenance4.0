using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Video : MonoBehaviour {

   // private string anim = "videoStep1.mp4", film = "filmStep1";
   // public int sceneNumber;

    // Use this for initialization
    void Start () {

    }

    /* private IEnumerator streamVideo(string video)
     {
         //Handheld.PlayFullScreenMovie(video, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFill);
         yield return new WaitForEndOfFrame();
         Debug.Log("The Video playback is now completed.");
        // SceneManager.LoadScene(sceneNumber);
     }*/

    /*public void playAnim()
    {
        GameObject camera = GameObject.Find("Main Camera");
        var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = "/Users/ECGM/Desktop/Android2D/assets/streamingassets/videoStep1.mp4";
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

    public void playVideo()
    {
        GameObject camera = GameObject.Find("Main Camera");
        var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.url = "/Users/ECGM/Desktop/Android2D/assets/streamingassets/filmStep1.mp4";
        videoPlayer.isLooping = true;
        videoPlayer.Play()
        ;
    }*/
    public void play()
    {
        StartCoroutine(playVideo());
    }

    public IEnumerator playAnim()
    {
        Handheld.PlayFullScreenMovie("videoStep1.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
        yield return new WaitForEndOfFrame();

        Debug.Log("The Video playback is now completed.");
    }

    public IEnumerator playVideo()
    {
        Handheld.PlayFullScreenMovie("filmStep1.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
        yield return new WaitForEndOfFrame();

    }


    
}
