using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager_procedureAnim : MonoBehaviour
{
    public Text nameText, dialogueText, text1, text2, text3;

    public Animator animator, peca_anim;

    public Queue<string> sentences;

    public GameObject peca2;

    public string sentence;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
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

        peca2 = GameObject.Find("peca_step2");

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        sentence = sentences.Dequeue();

        switch (sentences.Count)
        {
            case 0:
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
                SceneManager.LoadScene("final");
                break;
            case 1:
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
                text2.fontStyle = FontStyle.Normal;
                text3.fontStyle = FontStyle.Bold;
                peca_anim.SetBool("passo3", true);
                break;
            case 2:
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
                text1.fontStyle = FontStyle.Normal;
                text2.fontStyle = FontStyle.Bold;
                peca_anim.SetBool("passo2", true);
                break;
            case 3:
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
                text1.fontStyle = FontStyle.Bold;
                peca_anim.SetBool("hasStarted", true);
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
}
