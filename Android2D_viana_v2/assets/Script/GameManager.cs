using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public string username;


    public int maxMessages = 25;

    public GameObject chatPanel, textObject;
    public InputField chatBox;
    public Color playerMessage, info;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    void Start()
    {
        
    }

    public class MessageContent
    {
        public string username;
        public string message;        
        public MessageContent(string username,string message)
        {
            this.username = username;
            this.message = message;
        }
    }
    
    public void buttonClicked()
    {
        if (chatBox.text != "")
        {
            SendMessageToChat(username + ": " + chatBox.text, Message.MessageType.playerMessage);
            chatBox.text = "";
        }

        else SendMessageToChat("Info: empty message" , Message.MessageType.info);
    }

    IEnumerator chat()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToChat(username + ": " + chatBox.text, Message.MessageType.playerMessage);
           



            var dataToLaunch = JsonUtility.ToJson(new MessageContent(username, chatBox.text));
            print(dataToLaunch);


            chatBox.text = "";

            UnityWebRequest www = new UnityWebRequest("http://10.1.186.35:8080/api/talk", "POST");
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(new System.Text.UTF8Encoding().GetBytes(dataToLaunch));
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type","application/json");

            yield return www.SendWebRequest();
            

            var data = www.downloadHandler.text;
            print(data);

            var info = JsonUtility.FromJson<MessageContent>(data);
            

            SendMessageToChat(info.username+ ": " + info.message, Message.MessageType.info);

            // byte[] result = www.downloadHandler.data;


            //SendMessageToChat(result[, Message.MessageType.playerMessage);
        }
}

    void Update()
    {

        if(chatBox.text != "")
        {
            StartCoroutine(chat());
        }

        else
        {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
                chatBox.ActivateInputField();
        }

        if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("You pressed the space bar", Message.MessageType.info);
                Debug.Log("Space");
            }
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {

        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;

        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;

            case Message.MessageType.info:
                color = info;
                break;
        }

        return color;
    }

}

[System.Serializable]

public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info
    }
}
