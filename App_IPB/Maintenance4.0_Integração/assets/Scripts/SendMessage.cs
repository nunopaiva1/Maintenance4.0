﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PubNubAPI;
using UnityEngine.UI;
using System;

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
    //private TextMesh text;
    string username = "";
    //public GameObject panel;

    // Create a chat message queue so we can interate through all the messages
    Queue<GameObject> chatMessageQueue = new Queue<GameObject>();
   
    void Start()
    {
        username = PlayerPrefs.GetString("nome");
        // Use this for initialization
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.PublishKey = "pub-c-d3de4341-37e0-453b-b416-b099a148de7a";
        pnConfiguration.SubscribeKey = "sub-c-56be17da-8218-11e9-abf5-3aee3d8b0253";
        pnConfiguration.SecretKey = "sec-c-NzM3OGZkMDgtZTgxOS00NTYyLWJiZTQtZDNiZWI2NWVlZDEx";
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
                            Debug.Log("inside foreach");
                        // Format data into readable format
                        JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(pnMessageResult.Payload.ToString());

                        // Call the function to display the message in plain text
                        CreateChat(chatmessage);

                        // Counter used for positioning the text UI 
                        if (counter <= 0)
                        {
                            counter += 100;
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
            Debug.Log(message.MessageResult);
            if (message.MessageResult != null)
            {
                Debug.Log("Before Sync");
                // Format data into a readable format
                JSONInformation chatmessage = JsonUtility.FromJson<JSONInformation>(message.MessageResult.Payload.ToString());

                // Call the function to display the message in plain text
                CreateChat(chatmessage);

                // When a new chat is created, remove the first chat and transform all the messages on the page up
                SyncChat();

                
                // Counter used for position the text UI
                if (counter <= 0)
                {
                    counter += 100;
                }

                if (indexcounter > 1)
                {
                    foreach (GameObject moveChat in chatMessageQueue)
                    {
                        Debug.Log("Moving text");
                        RectTransform moveText = moveChat.GetComponent<RectTransform>();
                        moveText.offsetMax = new Vector2(moveText.offsetMax.x, moveText.offsetMax.y + 100);
                        moveText.sizeDelta = new Vector2(1875, 75);
                    }
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
        chatMessage.transform.SetParent(canvasObject.GetComponent<Canvas>().transform);
        chatMessage.AddComponent<Text>().text = currentObject;

        // Assign text to the gameobject. Add visual properties to text
        var chatText = chatMessage.GetComponent<Text>();
        chatText.font = customFont;
        chatText.color = Color.black;
        chatText.fontSize = 50;

        // Assign a RectTransform to gameobject to maniuplate positioning of chat.
        RectTransform rectTransform;
        rectTransform = chatText.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector2(0, -400 - counter); 
        rectTransform.sizeDelta = new Vector2(1875, 75); //15 1

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
            Debug.Log(chatMessageQueue);

            // Move all existing text gameobjects up the Y axis 50 pixels
            foreach (GameObject moveChat in chatMessageQueue)
            {
                Debug.Log("Moving text");
                RectTransform moveText = moveChat.GetComponent<RectTransform>();
                moveText.offsetMax = new Vector2(moveText.offsetMax.x, moveText.offsetMax.y + 100);
                moveText.sizeDelta = new Vector2(1875, 75);
            }
        }
        
    }

	// Update is called once per frame
	public void DeleteMessages () {
        pubnub.DeleteMessages()
    .Channel("chatchannel3")
    .Async((result, status) => {
        if (!status.Error)
        {
            Debug.Log(string.Format("DateTime {0}, In DeleteMessages Example, Timetoken: {1}", DateTime.UtcNow, result.Message));
        }
        else
        {
            Debug.Log(status.Error);
            Debug.Log(status.StatusCode);
            Debug.Log(status.ErrorData.Info);
        }
    });
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
