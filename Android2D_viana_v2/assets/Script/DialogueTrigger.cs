using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        Destroy(gameObject);
    }

    public void TriggerDialogue_anim()
    {
        FindObjectOfType<DialogueManager_procedureAnim>().StartDialogue(dialogue);
        Destroy(gameObject);
    }

}
