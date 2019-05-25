using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;

public class JSONInformation
{
    public string username;
    public string text;
}

public class SendMessage : MonoBehaviour {
    public static PubNub pubnub;
    public Font customFont;
    public Button SubmitButton;
    public Canvas canvasObject;
    public GameObject obj;
    public InputField UsernameInput;
    public InputField TextInput;
    public int counter = 0;
    public int indexcounter = 0;
    public Text deleteText;
    public Text moveTextUpwards;
    private TextMesh text;
    string username = "";
    public GameObject panel;

    // Create a chat message queue so we can interate through all the messages
    Queue<GameObject> chatMessageQueue = new Queue<GameObject>();
   
    void Start()
    {
        username = PlayerPrefs.GetString("nome");
        // Use this for initialization
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.PublishKey = "pub-c-7f1da7be-daa3-4333-a4f9-d530b9b2d6d2";
        pnConfiguration.SubscribeKey = "sub-c-2a796e96-7d62-11e9-912a-e2e853b4b660";
        pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
        pnConfiguration.UUID = System.Guid.NewGuid().ToString();
        pubnub = new PubNub(pnConfiguration);

        // Add Listener to Submit button to send messages
        Button btn = SubmitButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        // Fetch the last 13 messages sent on the given PubNub channel
        pubnub.FetchMessages()
            .Channels(new List<string> { "chatchannel3" })
            .Count(13)
            .Async((result, status) =>
            {
            if (status.Error)
            {
                Debug.Log(string.Format(
                    " FetchMessages Error: {0} {1} {2}", 
                    status.StatusCode, status.ErrorData, status.Category
                ));
            }
            else
            {
                foreach (KeyValuePair<string, List<PNMessageResult>> kvp in result.Channels)
                {
                    foreach (PNMessageResult pnMessageResult in kvp.Value)
                    {
                        // Format data into readable format
                        JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(pnMessageResult.Payload.ToString());

                        // Call the function to display the message in plain text
                        CreateChat(chatmessage);

                        // Counter used for positioning the text UI 
                        if (counter != 650)
                        {
                            counter += 50;
                        }
                    }
                 }
             }
             });

        // Subscribe to a PubNub channel to receive messages when they are sent on that channel
        pubnub.Subscribe()
            .Channels(new List<string>() {
                "chatchannel3"
            })
            .WithPresence()
            .Execute();

        // This is the subscribe callback function where data is recieved that is sent on the channel
        pubnub.SusbcribeCallback += (sender, e) =>
        {
            SusbcribeEventEventArgs message = e as SusbcribeEventEventArgs;
            if (message.MessageResult != null)
            {
                // Format data into a readable format
                JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(message.MessageResult.Payload.ToString());

                // Call the function to display the message in plain text
                CreateChat(chatmessage);

                // When a new chat is created, remove the first chat and transform all the messages on the page up
                SyncChat();

                // Counter used for position the text UI
                if (counter != 650)
                {
                    counter += 50;
                }
            }
        };
    }

    // Function used to create new chat objects based of the data received from PubNub
    void CreateChat(JSONInformation payLoad){

        // Create a string with the username and text
        string currentObject = string.Concat(payLoad.username, payLoad.text);

        // Create a new gameobject that will display text of the data sent via PubNub
        GameObject chatMessage = new GameObject(currentObject);
        chatMessage.transform.SetParent(canvasObject.transform);
        chatMessage.AddComponent<TextMesh>().text = currentObject;

        // Assign text to the gameobject. Add visual properties to text
        //var chatText = chatMessage.GetComponent<Text>();
        //chatText.font = customFont;
        //chatText.color = Color.black;
        //chatText.fontSize = 7;

        //chatText.alignment = TextAnchor.LowerLeft;

        // Assign a RectTransform to gameobject to maniuplate positioning of chat.
        var chatText = chatMessage.GetComponent<TextMesh>();
        RectTransform rectTransform;
        rectTransform = chatText.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector2(0, -400 - counter); // 0 -400
        rectTransform.sizeDelta = new Vector2(125, 11); //15 1
     // rectTransform.localScale -= new Vector3(15, 15, 15);

        MeshRenderer mesh;
        mesh = chatText.GetComponent<MeshRenderer>();
        TextMesh mesh1;
        mesh1 = chatText.GetComponent<TextMesh>();
        mesh1.font = customFont;
        mesh1.fontSize = 30;
        
       
        //rectTransform.rotation = Quaternion.Euler(0, 0, 180);

        // Assign the gameobject to the queue of chatmessages
        chatMessageQueue.Enqueue(chatMessage);

        // Keep track of how many objects we have displayed on the screen
        indexcounter++;
    }

    void SyncChat() {
        // If more 13 objects are on the screen, we need to start removing them
        if (indexcounter >= 14)
        {
            // Delete the first gameobject in the queue
            GameObject deleteChat = chatMessageQueue.Dequeue();
            Destroy(deleteChat);

            // Move all existing text gameobjects up the Y axis 50 pixels
            foreach (GameObject moveChat in chatMessageQueue)
            {
                RectTransform moveText = moveChat.GetComponent<RectTransform>();
                moveText.offsetMax = new Vector2(moveText.offsetMax.x, moveText.offsetMax.y + 50);
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}

    void TaskOnClick()
    {
        // When the user clicks the Submit button,
        // create a JSON object from input field input
        JSONInformation publishMessage = new JSONInformation();
        publishMessage.username = string.Concat(username, ": ");//UsernameInput.text
        publishMessage.text = TextInput.text;
        string publishMessageToJSON = JsonUtility.ToJson(publishMessage);

        // Publish the JSON object to the assigned PubNub Channel
        pubnub.Publish()
            .Channel("chatchannel3")
            .Message(publishMessageToJSON)
            .Async((result, status) =>
            {
                if (status.Error)
                {
                    Debug.Log(status.Error);
                    Debug.Log(status.ErrorData.Info);
                }
                else
                {
                    Debug.Log(string.Format("Publish Timetoken: {0}", result.Timetoken));
                }
            });

        TextInput.text = "";
    }
}
