using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    /*void OnMouseDown()
    {
        Debug.Log("Click");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    } */
}
