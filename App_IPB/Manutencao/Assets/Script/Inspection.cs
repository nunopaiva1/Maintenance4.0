using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Inspection : MonoBehaviour, ITrackableEventHandler
{

    TrackableBehaviour mTrackableBehaviour;

    const string pluginName = "com.cwgtech.unity.MyPlugin";

    class AlertViewCallback : AndroidJavaProxy
    {
        private System.Action<int> alertHandler;

        public AlertViewCallback(System.Action<int> alertHandlerIn) : base(pluginName + "$AlertViewCallback")
        {
            alertHandler = alertHandlerIn;
        }
        public void onButtonTapped(int index)
        {
            Debug.Log("Button tapped: " + index);
            if (alertHandler != null)
                alertHandler(index);
        }
    }

    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;
    public RectTransform webPanel;
    public RectTransform buttonStrip;

    public static AndroidJavaClass PluginClass
    {
        get
        {
            if (_pluginClass == null)
            {
                _pluginClass = new AndroidJavaClass(pluginName);
                AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                _pluginClass.SetStatic<AndroidJavaObject>("mainActivity", activity);
            }
            return _pluginClass;
        }
    }

    public static AndroidJavaObject PluginInstance
    {
        get
        {
            if (_pluginInstance == null)
            {
                _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
            }
            return _pluginInstance;
        }
    }
    // Use this for initialization
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED)
        {
            OpenWebViewTapped();
        }
        //else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NOT_FOUND)
        //{
        //    CloseWebViewTapped();
        //}
    }

    public void OpenWebView(string URL, int pixelShift)
    {
        if (Application.platform == RuntimePlatform.Android)
            PluginInstance.Call("showWebView", new object[] { URL, pixelShift });
    }

    public void CloseWebView(System.Action<int> closeComplete)
    {
        if (Application.platform == RuntimePlatform.Android)
            PluginInstance.Call("closeWebView");//, new object[]{ new ShareImageCallback(closeComplete) }
        else
            closeComplete(0);
    }

    public void OpenWebViewTapped()
    {
            Canvas parentCanvas = buttonStrip.GetComponentInParent<Canvas>();
            int stripHeight = (int)(buttonStrip.rect.height * parentCanvas.scaleFactor + 0.5f);
            webPanel.gameObject.SetActive(true);
            OpenWebView("http://192.168.217.179:1880/ui/#!/2", stripHeight);
    }


    public void CloseWebViewTapped()
    {
            CloseWebView((int result) =>
            {
                webPanel.gameObject.SetActive(false);
            });
    }
}
