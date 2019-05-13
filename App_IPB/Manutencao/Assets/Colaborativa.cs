using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class Colaborativa : MonoBehaviour, ITrackableEventHandler
{

    TrackableBehaviour image_TrackableBehaviour;
    VuMarkManager I401;

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
        image_TrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (image_TrackableBehaviour != null)
        {
            image_TrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        I401 = TrackerManager.Instance.GetStateManager().GetVuMarkManager();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OpenWebViewTapped();
            //Application.OpenURL("http://192.168.217.178:1880");
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED && newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            CloseWebViewTapped();
        }
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

    void OpenWebViewTapped()
    {
        foreach (var item in I401.GetActiveBehaviours())
        {
            Canvas parentCanvas = buttonStrip.GetComponentInParent<Canvas>();
            int stripHeight = (int)(buttonStrip.rect.height * parentCanvas.scaleFactor + 0.5f);
            webPanel.gameObject.SetActive(true);
            OpenWebView("http://192.168.217.178:1880/ui/#/3", stripHeight);
            //webPanel.gameObject.SetActive(true);
            // WebViewObject webViewObject;
            // webViewObject =
            //(new GameObject("WebViewObject")).AddComponent<WebViewObject>();
            // webViewObject.SetMargins(5, 5, 5, 40);
            // webViewObject.SetVisibility(true);

            // webViewObject.LoadURL("file://" + "http://192.168.217.178:1880");
        }
    }


    void CloseWebViewTapped()
    {
        foreach (var item in I401.GetActiveBehaviours())
        {
            CloseWebView((int result) =>
            {
                webPanel.gameObject.SetActive(false);
            });
        }
    }
}


