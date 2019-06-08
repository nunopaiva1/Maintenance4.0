using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class Parsetest : MonoBehaviour
{
    public Text txtbox;
    string nome = "";
    string click = "";
    public Text uiText;
    public Button btn1;
    public Button no;
    private string fileName;
    private string path;
    public XmlReader reader2;
    public XmlReader reader1;
    private string media = "";
    private string media1 = "";
    private string Step = "";
    private string filename = "";
    public VideoClip videoClips;
    private VideoPlayer videoPlayer;
    private VideoSource videoSource;
    public Texture Imagem;
    public RawImage image;
    Texture2D myTexture;
    public GameObject popup;
    public GameObject popup1;
    Dictionary<string, Task> tasklist = new Dictionary<string, Task>();
    List<Secflow> flowlist = new List<Secflow>();
    List<Property> prop = new List<Property>();
    Property curentProp;
    Secflow curentElement;
    // Start is called before the first frame update
    void Start()
    {
        fileName = PlayerPrefs.GetString("filename"); //Quando foi escolhido o procedimento, foi guardado o nome para depois ir buscar o XML automaticamente com este filename
        filename = fileName.Replace(".xml", "");
        Debug.Log(filename);
        txtbox.text = filename;

        Button Seguinte = btn1.GetComponent<Button>();

        path = GetPath();
        reader2 = XmlReader.Create(path);
        reader1 = XmlReader.Create(path);

        while (reader2.Read()) // fill thy list
        {
            //Preencher a lista com os sequence flows do XML
            if ((reader2.NodeType == XmlNodeType.Element) && (reader2.Name == "bpmn:sequenceFlow"))
            {
                flowlist.Add(new Secflow(reader2.GetAttribute("id"),
                                reader2.GetAttribute("sourceRef"),
                                reader2.GetAttribute("targetRef"),
                                reader2.GetAttribute("name")));
            }
        }

        reader1 = XmlReader.Create(path);
        while (reader1.Read()) // fill thy list
        {
            //Preencher a lista com as propriedades de cada tarefa
            if ((reader1.NodeType == XmlNodeType.Element) && (reader1.Name == "camunda:property"))
            {
                prop.Add(new Property(reader1.GetAttribute("id"),
                                reader1.GetAttribute("name"),
                                reader1.GetAttribute("value")));
            }
        }

        reader2 = XmlReader.Create(path);
        while (reader2.Read()) // fill thy dictionary
        {
            //preencher dicionario com todas as tarefas do procedimento
            if ((reader2.NodeType == XmlNodeType.Element) && (reader2.Name == "bpmn:task"))
            {
                tasklist.Add(reader2.GetAttribute("id"),
                            new Task(reader2.GetAttribute("id"),
                                    reader2.GetAttribute("name")));
            }

        }
        curentElement = flowlist[0]; //Primeiro elemento do flowlist
        curentProp = prop[0];
    
}

    public void BtnTest()
    {
        //Ao clicar nos butões, é chamada est função que depois chama a função Myf() e também é onde vê qual dos butoes foi clicado com a variavél click
        click = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(click);
        Myf();

    }

    public void Myf()
    {

        if (curentElement.tr.StartsWith("ExclusiveGateway")) //So entra neste if se o elemento for um exlusive gateway
        {
            string curentTask = curentElement.tr;
            uiText.text = curentTask;

            if (click == "Seguinte")
            {
                Debug.Log("Dentro seguinte com exclusive");
                try
                {
                    curentElement = GetNext(flowlist, curentElement.tr, "Sim");
                    string task = tasklist[curentElement.tr].name;
                    uiText.text = task; //Aqui faz o display das tarefas para o utilizador

                    //Esta lista reune todas as propriedades de cada tarefa, como o stepcount e se é video ou imagem e guarda os em variáveis, para ser utilizado a seguir
                    List<Property> TaskProp = GetAllProp(prop, tasklist[curentElement.tr].id);
                    for (int i = 0; i < TaskProp.Count; i++)
                    {
                        if (TaskProp[i].name == "StepCount")
                        {
                            Step = TaskProp[i].value;
                        }
                        else if ((TaskProp[i].name == "Video") || ((TaskProp[i].name == "Image")) || ((TaskProp[i].name == "VidIm")))
                        {
                            media1 = TaskProp[i].name;
                            media = TaskProp[i].value;
                            Debug.Log("media1 = " + media1);
                            Debug.Log("media = " + media);
                        }
                    }

                    //Se for video, o butão help fica ativo e coloca a imagem do passo em que está na RawImage que tem na cena e se for imagem o butão fica inativo e coloca na RawImage a imagem do passo em que está
                    if (media.StartsWith("P"))
                    {
                        if (media1 == "Video")
                        {
                            Debug.Log("inside video");
                           // mybutton.gameObject.SetActive(true);
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                        else if (media1 == "Image")
                        {
                            Debug.Log("inside image");
                            //mybutton.gameObject.SetActive(false);
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                        else if (media1 == "VidIm")
                        {
                            Debug.Log("Inside VidIm");
                            //mybutton.gameObject.SetActive(true);
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                    }

                }
                catch
                {
                    
                }
            }
            // Aqui é igual ao que estava em cima mas se for carregado o botão Não
            else if (click == "Anterior")
            {
                Debug.Log("Dentro anterior");
                try
                {
                    curentElement = GetNext(flowlist, curentElement.tr, "Não");
                    string task = tasklist[curentElement.tr].name;
                    uiText.text = task;

                    List<Property> TaskProp = GetAllProp(prop, tasklist[curentElement.tr].id);
                    for (int i = 0; i < TaskProp.Count; i++)
                    {
                        if (TaskProp[i].name == "StepCount")
                        {
                            Step = TaskProp[i].value;
                        }
                        else if ((TaskProp[i].name == "Video") || ((TaskProp[i].name == "Image")) || ((TaskProp[i].name == "VidIm")))
                        {
                            media1 = TaskProp[i].name;
                            media = TaskProp[i].value;
                            Debug.Log("media1 = " + media1);
                            Debug.Log("media = " + media);
                        }
                    }

                    if (media.StartsWith("P"))
                    {
                        if (media1 == "Video")
                        {
                            Debug.Log("inside video");
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                        else if (media1 == "Image")
                        {
                            Debug.Log("inside image");
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                        else if (media1 == "VidIm")
                        {
                            Debug.Log("Inside VidIm");
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                    }
                    curentElement = GetNext(flowlist, curentElement.tr, null);
                }
                catch
                {

                }
            }
        }

        //Entra neste IF se foi carregado o botão Sim e o elemento em que está situado não seja um exclusive gateway e o código é semelhante ao de cima
        else if (click == "Seguinte")
        {
            Debug.Log("dentro seguinte sem exclusive");
            try
            {
                string task = tasklist[curentElement.tr].name;
                uiText.text = task;

                List<Property> TaskProp = GetAllProp(prop, tasklist[curentElement.tr].id);
                for (int i = 0; i < TaskProp.Count; i++)
                {
                    Debug.Log("for");
                    if (TaskProp[i].name == "StepCount")
                    {
                        Step = TaskProp[i].value;
                        Debug.Log(Step);
                    }
                    else if ((TaskProp[i].name == "Video") || ((TaskProp[i].name == "Image")) || ((TaskProp[i].name == "VidIm")))
                    {
                        media1 = TaskProp[i].name;
                        media = TaskProp[i].value;
                        Debug.Log("media1 = " + media1);
                        Debug.Log("media = " + media);
                    }
                }

                if (media.StartsWith("P"))
                {
                    if (media1 == "Video")
                    {
                        Debug.Log("inside video");
                        myTexture = Resources.Load("Sprites/" + filename + "/Vid/" + media) as Texture2D;
                        GameObject rawImage = GameObject.Find("RawImage");
                        rawImage.GetComponent<RawImage>().texture = myTexture;
                    }
                    else if (media1 == "Image")
                    {
                        Debug.Log("inside image");
                        myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                        GameObject rawImage = GameObject.Find("RawImage");
                        rawImage.GetComponent<RawImage>().texture = myTexture;
                    }
                    else if (media1 == "VidIm")
                    {
                        Debug.Log("Inside VidIm");
                        myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                        GameObject rawImage = GameObject.Find("RawImage");
                        rawImage.GetComponent<RawImage>().texture = myTexture;
                    }
                }

                curentElement = GetNext(flowlist, curentElement.tr, null);
                if (curentElement.tr.StartsWith("ExclusiveGateway"))
                {
                    no.interactable = true;
                }
            }


            catch
            {
                Debug.Log("catch");
            }
        }
    }

    static public Secflow GetNext(List<Secflow> flowlist, string src, string option)
    {
        for (int i = 0; i < flowlist.Count; i++)
        {
            if ((flowlist[i].sr == src) && (flowlist[i].name == option))
            {
                return (flowlist[i]);
            }
        }
        return (null);
    }

    static public List<Property> GetAllProp(List<Property> prop, string point)
    {
        List<Property> SpecificTest = new List<Property>();
        for (int i = 0; i < prop.Count; i++)
        {
            if ((prop[i].id == point))
            {
                SpecificTest.Add(prop[i]);
            }
        }
        return SpecificTest;
    }

    public class Secflow
    {
        public string id;
        public string sr;
        public string tr;
        public string name;

        public Secflow(string id, string sr, string tr, string name)
        {
            this.id = id;
            this.sr = sr;
            this.tr = tr;
            this.name = name;
        }

    }

    public class Property
    {
        public string id;
        public string name;
        public string value;

        public Property(string id, string name, string value)
        {
            this.id = id;
            this.name = name;
            this.value = value;
        }
    }

    public class Task
    {
        public string id;
        public string name;

        public Task(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }

    private string GetPath()
    {
        Debug.Log("FILE_NAME: " + fileName);
        string assetPath = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            assetPath = Application.persistentDataPath + "/Resources/" + fileName;
        }
        // iPhone
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            assetPath = Application.dataPath + "/Raw" + fileName;
        }
        // mac or windows
        else
        {
            assetPath = Application.dataPath + "/Resources/" + fileName;

        }
        return assetPath;
    }

    public void Image()
    {
        image.texture = Imagem;
    }

    public IEnumerator Video()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.clip = videoClips;
        videoPlayer.Prepare();
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitTime;
            break;
        }
        image.texture = videoPlayer.texture;
        videoPlayer.Play();
    }

    public void Close()
    {
        popup.gameObject.SetActive(false);
        popup1.gameObject.SetActive(false);
    }
    public void Open()
    {
        popup.gameObject.SetActive(true);
        popup1.gameObject.SetActive(true);
    }
}
