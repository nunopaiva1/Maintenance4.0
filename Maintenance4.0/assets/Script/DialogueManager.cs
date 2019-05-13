using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    public Text nameText, dialogueText, text1, text2, text3, timeText;

    public Animator animator;

    public Queue<string> sentences;

    public GameObject vbBtnObj, mark, cube, peca2, panel, video;

    public string sentence;

    public Camera AR_cam, main_cam;

    float Timer = 0.0f;
    float Min = 0.0f;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
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

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        mark = GameObject.Find("mark");
        mark.SetActive(true);

        cube = GameObject.Find("step1");
        cube.SetActive(false);

        vbBtnObj = GameObject.Find("botaoVirtual");
        vbBtnObj.SetActive(false);

        peca2 = GameObject.Find("peca_step2");
        peca2.SetActive(false);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        sentence = sentences.Dequeue();

        switch (sentences.Count)
        {
            case 0:
                StopAllCoroutines();
                EndDialogue();
                break;
            case 1:
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
                SceneManager.LoadScene("final");
                break;
            case 2:
                //StopAllCoroutines();
                panel.SetActive(true);
                //StartCoroutine(TypeSentence(sentence));
                vbBtnObj.SetActive(false);
                cube.SetActive(false);
                break;
            case 3:
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
                mark.SetActive(false);
                cube.SetActive(true);
                text1.fontStyle = FontStyle.Normal;
                text2.fontStyle = FontStyle.Bold;
                break;
            case 4:
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
                vbBtnObj.SetActive(true);
                mark.SetActive(true);
                text1.fontStyle = FontStyle.Bold;
                break;
        }
    

            Debug.Log(sentences.Count);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

   public void goodState()
    {
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        vbBtnObj.SetActive(true);
        peca2.SetActive(true);

        panel.SetActive(false);

        text2.fontStyle = FontStyle.Normal;
        text3.fontStyle = FontStyle.Bold;

        sentences.Enqueue("");
    }

    public void badState()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        panel.SetActive(false);
        text2.fontStyle = FontStyle.Normal;
    }

    public void showAnimation()
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
