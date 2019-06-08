using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    public GameObject close;
    public GameObject text;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void NewProcedure()
    {
        Application.OpenURL("https//:");
    }

    public void Defects()
    {
        //AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //AndroidJavaObject pm = jo.Call<AndroidJavaObject>("getPackageManager");
        //AndroidJavaObject intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", "com.x.x");

        //jo.Call("startActivity", intent);

        //Application.OpenURL("http://maintenance4.estig.ipb.pt/CatraportApp/index.php");
        OpenWebViewTapped();
    }

    public void Procedures()
    {
        SceneManager.LoadScene("FirstPage");
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
        //text.gameObject.SetActive(false);
        webPanel.gameObject.SetActive(true);
        close.gameObject.SetActive(true);
        OpenWebView("http://maintenance4.estig.ipb.pt/CatraportApp/index.php", stripHeight);
    }


    public void CloseWebViewTapped()
    {
        CloseWebView((int result) =>
        {
            webPanel.gameObject.SetActive(false);
            close.gameObject.SetActive(false);
            //text.gameObject.SetActive(true);
        });
    }
}
