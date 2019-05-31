using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using MySql.Data.MySqlClient;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Parser : MonoBehaviour
{
    public Text txtbox;
    public GameObject panel;
    public Text timeText;
    //public Animator animator;
    public GameObject vbBtnObj, mark, cube, peca2, video;
    public Camera AR_cam, main_cam;
    float Timer = 0.0f;
    float Min = 0.0f;

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
    public GameObject obj;
    public InputField input;

    public VideoClip videoClips;
    private VideoPlayer videoPlayer;
    private VideoSource videoSource;
    //public RawImage ima;
    //public Texture imvid;
    public Texture Imagem;
    public RawImage image;
    public Button mybutton;
    public GameObject HelpPopUp;
    //public GameObject Im;
    Texture2D myTexture;

    Dictionary<string, Task> tasklist = new Dictionary<string, Task>();
    List<Secflow> flowlist = new List<Secflow>();
    List<Property> prop = new List<Property>();
    Property curentProp;
    Secflow curentElement;

    // Use this for initialization
    void Start()
    {

        nome = PlayerPrefs.GetString("name"); 
        Debug.Log(nome);
        fileName = PlayerPrefs.GetString("filename"); //Quando foi escolhido o procedimento, foi guardado o nome para depois ir buscar o XML automaticamente com este filename
        filename = fileName.Replace(".xml", "");
        Debug.Log(filename);
        txtbox.text = filename;

        string date = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
        string time = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
        string ID1 = filename + date + "-" + time; //ID do procedimento para colocar na BD

        try
        {
            //Aqui introduzimos a informação do ID, data e hora de inicio na mesma linha onde está o nome do utilizador 
            string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
            //This is my insert query in which i am taking input from the user through windows forms  
            string Query = "UPDATE catraport.Procedimento" +
                " SET Id= '" + ID1 + "', Data= '" + date + "', HoraInício ='" + time + "', Procedimento = '" + filename + "'" +
                " WHERE utilizador = '" + nome + "';";
            //string Query = "insert into catraport.Procedimento(Id, Data, HoraInício) values('" + ID1 + "','" + date + "','" + time + "');";
            //This is  MySqlConnection here i have created the object and pass my connection string.  
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            //This is command class which will handle the query and connection object.  
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
            MyConn2.Open();
            MyCommand2.ExecuteNonQuery();
            MyConn2.Close();
            //
        }
        catch
        {
            Debug.Log("Erro na base de dados");
        }

        Button yes = btn1.GetComponent<Button>();

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

    private void Update()
    {
        if (Timer > 9) timeText.text = "Time: 0" + Min + ":" + System.Math.Round(Timer, 0);
        else timeText.text = "Time: 0" + Min + ":0" + System.Math.Round(Timer, 0);

        if (Timer > 59)
        {
            Min += 1;
            Timer = 0;
        }

        Timer += Time.deltaTime;
    }

    public void BtnTest()
    {
        //Ao clicar nos butões, é chamada est função que depois chama a função Myf() e também é onde vê qual dos butoes foi clicado com a variavél click
        click = EventSystem.current.currentSelectedGameObject.name;
        if ((click == "No")) // Aqui é onde aparece o pop-up para o utilizador introduzir a informação do porquê de não conseguir realizar a tarefa se clicar no butão X onde não exista alguma exclusive gateway
        {
            input.text = "";
            obj.SetActive(true);
            var se = new InputField.SubmitEvent();
            se.AddListener(SubmitText);
            input.onEndEdit = se;
            
        }
        panel.SetActive(false);
        Myf();

    }


    public void Myf()
    {

        if (curentElement.tr.StartsWith("ExclusiveGateway")) //So entra neste if se o elemento for um exlusive gateway
        {
            Debug.Log("ex1 " + curentElement.tr);
            string curentTask = curentElement.tr;
            uiText.text = curentTask;

            if (click == "Yes")
            {
                try
                {


                    string date = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
                    string time = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

                    curentElement = GetNext(flowlist, curentElement.tr, "Sim");
                    Debug.Log("terget: " + "yes " + tasklist[curentElement.tr].name);
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
                        }
                    }
                    Debug.Log("Step is = " + Step);
                    Debug.Log("media is = " + media);

                    //Se for video, o butão help fica ativo e coloca a imagem do passo em que está na RawImage que tem na cena e se for imagem o butão fica inativo e coloca na RawImage a imagem do passo em que está
                    if (media.StartsWith("P"))
                    {
                        if (media1 == "Video")
                        {
                            Debug.Log("inside video");
                            mybutton.gameObject.SetActive(true);
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                        else if (media1 == "Image")
                        {
                            Debug.Log("inside image");
                            mybutton.gameObject.SetActive(false);
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                        else if (media1 == "VidIm")
                        {
                            Debug.Log("Inside VidIm");
                            mybutton.gameObject.SetActive(true);
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                    }

                    try
                    {
                        // Aqui insere na BD, em cada tarefa, um ID, data, hora de inicio da tarefa e o numero dela.
                        string ID1 = filename + "-" + date + "-" + time;
                        string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
                        //This is my insert query in which i am taking input from the user through windows forms  
                        string Query = "INSERT INTO catraport.Procedimento(Id, Data, HoraInício,StepCount) VALUES('" + ID1 + "','" + date + "','" + time + "','" + Step + "');";
                        //This is  MySqlConnection here i have created the object and pass my connection string.  
                        MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                        //This is command class which will handle the query and connection object.  
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                        MyConn2.Open();
                        MyCommand2.ExecuteNonQuery();
                        MyConn2.Close();
                    }
                    catch
                    {
                        Debug.Log("Erro na base de dados");
                    }



                }
                catch
                {
                    try
                    {
                        //Quando finaliza o procedimento muda de cena e insere informação na BD
                        string time1 = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        SceneManager.LoadScene("TaskFinished", LoadSceneMode.Additive);
                        string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
                        //This is my insert query in which i am taking input from the user through windows forms  
                        //string Query = "INSERT INTO catraport.Procedimento(utilizador, Data, HoraInício, HoraFim, StepCount) VALUES('" + putsomething + "','" + date + "','" + starttime + "','" + endtime + "','" + step + "');";
                        // string Query = "INSERT INTO catraport.Procedimento(HoraFim) VALUES('" + time1 + "');";
                        string Query = "UPDATE catraport.Procedimento SET HoraFim ='" + time1 + "' WHERE StepCount = " + Step + ";";
                        //This is  MySqlConnection here i have created the object and pass my connection string.  
                        MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                        //This is command class which will handle the query and connection object.  
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                        MyConn2.Open();
                        MyCommand2.ExecuteNonQuery();
                        MyConn2.Close();
                    }
                    catch
                    {
                        Debug.Log("Erro na base de dados");
                    }
                }
            }


            // Aqui é igual ao que estava em cima mas se for carregado o botão Não
            else if (click == "No")
            {
                string date = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
                string time = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

                try
                {
                    obj.SetActive(false);
                    curentElement = GetNext(flowlist, curentElement.tr, "Não");
                    Debug.Log("terget: " + "no " + tasklist[curentElement.tr].name);
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
                            media = TaskProp[i].value;
                        }
                    }
                    Debug.Log("Step is = " + Step);
                    Debug.Log("media is = " + media);

                    if (media.StartsWith("P"))
                    {
                        if (media1 == "Video")
                        {
                            Debug.Log("inside video");
                            mybutton.gameObject.SetActive(true);
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                        else if (media1 == "Image")
                        {
                            Debug.Log("inside image");
                            mybutton.gameObject.SetActive(false);
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                        else if (media1 == "VidIm")
                        {
                            Debug.Log("Inside VidIm");
                            mybutton.gameObject.SetActive(true);
                            myTexture = Resources.Load("Sprites/" + filename + "/" + media) as Texture2D;
                            GameObject rawImage = GameObject.Find("RawImage");
                            rawImage.GetComponent<RawImage>().texture = myTexture;
                        }
                    }
                    
                    string ID1 = filename + "-" + date + "-" + time;

                    try
                    {
                        string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
                        //This is my insert query in which i am taking input from the user through windows forms  
                        //string Query = "INSERT INTO catraport.Procedimento(utilizador, Data, HoraInício, HoraFim, StepCount) VALUES('" + putsomething + "','" + date + "','" + starttime + "','" + endtime + "','" + step + "');";
                        string Query = "INSERT INTO catraport.Procedimento(iD, Data, HoraInício,StepCount) VALUES('" + ID1 + "','" + date + "','" + time + "','" + Step + "');";
                        //This is  MySqlConnection here i have created the object and pass my connection string.  
                        MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                        //This is command class which will handle the query and connection object.  
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                        MyConn2.Open();
                        MyCommand2.ExecuteNonQuery();
                        MyConn2.Close();
                    }
                    catch
                    {
                        Debug.Log("Erro na base de dados");
                    }

                    curentElement = GetNext(flowlist, curentElement.tr, null);
                }
                catch
                {
                    try
                    {
                        string time1 = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        SceneManager.LoadScene("TaskFinished", LoadSceneMode.Additive);
                        string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
                        //This is my insert query in which i am taking input from the user through windows forms  
                        //string Query = "INSERT INTO catraport.Procedimento(utilizador, Data, HoraInício, HoraFim, StepCount) VALUES('" + putsomething + "','" + date + "','" + starttime + "','" + endtime + "','" + step + "');";
                        //string Query = "INSERT INTO catraport.Procedimento(HoraFim) VALUES('" + time1 + "');";
                        string Query = "UPDATE catraport.Procedimento SET HoraFim ='" + time1 + "' WHERE StepCount = " + Step + ";";
                        //This is  MySqlConnection here i have created the object and pass my connection string.  
                        MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                        //This is command class which will handle the query and connection object.  
                        MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                        MyConn2.Open();
                        MyCommand2.ExecuteNonQuery();
                        MyConn2.Close();
                    }
                    catch
                    {
                        Debug.Log("Erro na base de dados");
                    }
                }
            }
        }

        //Entra neste IF se foi carregado o botão Sim e o elemento em que está situado não seja um exclusive gateway e o código é semelhante ao de cima
        else if (click == "Yes")
        {
            string date = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            string time = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;

            try
            {
                Debug.Log("terget: " + "all " + tasklist[curentElement.tr].name);
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
                    }
                }
                Debug.Log("Step is = " + Step);
                Debug.Log("media is = " + media);

                
                if (media.StartsWith("P"))
                {
                    if (media1 == "Video")
                    {
                        Debug.Log("inside video");
                        mybutton.gameObject.SetActive(true);
                        myTexture = Resources.Load("Sprites/" + filename + "/Vid/" + media) as Texture2D;
                        GameObject rawImage = GameObject.Find("RawImage");
                        rawImage.GetComponent<RawImage>().texture = myTexture;
                    }
                    else if (media1 == "Image")
                    {
                        Debug.Log("inside image");
                        mybutton.gameObject.SetActive(false);
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

                curentElement = GetNext(flowlist, curentElement.tr, null);
                if (curentElement.tr.StartsWith("ExclusiveGateway"))
                {
                    no.interactable = true;
                }

                string ID1 = filename + "-" + date + "-" + time;
                try
                {
                    string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
                    //This is my insert query in which i am taking input from the user through windows forms  
                    //string Query = "INSERT INTO catraport.Procedimento(utilizador, Data, HoraInício, HoraFim, StepCount) VALUES('" + putsomething + "','" + date + "','" + starttime + "','" + endtime + "','" + step + "');";
                    string Query = "INSERT INTO catraport.Procedimento(Id, Data, HoraInício,StepCount) VALUES('" + ID1 + "','" + date + "','" + time + "','" + Step + "');";
                    //This is  MySqlConnection here i have created the object and pass my connection string.  
                    MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                    //This is command class which will handle the query and connection object.  
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                    MyConn2.Open();
                    MyCommand2.ExecuteNonQuery();
                    MyConn2.Close();
                }
                catch
                {
                    Debug.Log("Erro na base de dados");
                }
            }
            catch
            {
                try
                {
                    string time1 = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                    //SceneManager.LoadScene("final", LoadSceneMode.Additive);
                    Application.LoadLevel("final");
                    string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
                    //This is my insert query in which i am taking input from the user through windows forms  
                    //string Query = "INSERT INTO catraport.Procedimento(utilizador, Data, HoraInício, HoraFim, StepCount) VALUES('" + putsomething + "','" + date + "','" + starttime + "','" + endtime + "','" + step + "');";
                    string Query = "UPDATE catraport.Procedimento SET HoraFim ='" + time1 + "' WHERE StepCount = " + Step + ";";
                    //string Query = "INSERT INTO catraport.Procedimento(HoraFim) VALUES('" + time1 + "');";
                    //This is  MySqlConnection here i have created the object and pass my connection string.  
                    MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
                    //This is command class which will handle the query and connection object.  
                    MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
                    MyConn2.Open();
                    MyCommand2.ExecuteNonQuery();
                    MyConn2.Close();
                }
                catch
                {
                    Debug.Log("Erro na base de dados");
                }
            }
        }
    }

    //Função utilizada para inserir o texto escrito pelo utilizador do porque de não conseguir realizar uma tarefa na BD
    public void SubmitText(string text)
    {
        panel.SetActive(true);
        Debug.Log(text);
        obj.SetActive(false);
        try
        {
            string MyConnection2 = "datasource=193.136.195.23;port=3306;username=IPA;password=cedri#2018";
            //This is my insert query in which i am taking input from the user through windows forms  
            //string Query = "INSERT INTO catraport.Procedimento(Motivo) VALUES('" + text + "');";
            string Query = "UPDATE catraport.Procedimento SET MOTIVO ='" + text + "' WHERE StepCount = " + Step + ";";
            //This is  MySqlConnection here i have created the object and pass my connection string.  
            MySqlConnection MyConn2 = new MySqlConnection(MyConnection2);
            //This is command class which will handle the query and connection object.  
            MySqlCommand MyCommand2 = new MySqlCommand(Query, MyConn2);
            MyConn2.Open();
            MyCommand2.ExecuteNonQuery();
            MyConn2.Close();
        }
        catch
        {
            Debug.Log("Erro na base de dados");
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

    public void Quit()
    {
        obj.SetActive(false);
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
            Debug.Log("ddd");
            assetPath = Application.dataPath + "/Resources/" + fileName;
            
        }
        return assetPath;
    }

    public void Image()
    {
        image.texture = Imagem;
    }


    public void Help()
    {
        HelpPopUp.SetActive(true);
        StartCoroutine(Video());
    }

    public void CloseWindow()
    {
        Destroy(videoPlayer);
        HelpPopUp.SetActive(false);
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

    public void ShowAnimation()
    {
        AR_cam.enabled = !AR_cam.enabled;
        main_cam.enabled = !main_cam.enabled;

        var someScript = GameObject.FindGameObjectWithTag("video").GetComponent<Video>();
        StartCoroutine(someScript.playAnim());

        AR_cam.enabled = !AR_cam.enabled;
        main_cam.enabled = !main_cam.enabled;

        /*if (main_cam.enabled)
        {
            //video.SetActive(true);
            Debug.Log("video reproduziu");
            GameObject.Find("btn_showAnim").GetComponentInChildren<Text>().text = "Show AR camera";

            var someScript = GameObject.FindGameObjectWithTag("video").GetComponent<Video>();
            StartCoroutine(someScript.playAnim());
        }
        else
        {
            //video.SetActive(false);
            Debug.Log("video está desligado");
            GameObject.Find("btn_showAnim").GetComponentInChildren<Text>().text = "Show 3D animation";
        }*/
    }
}
