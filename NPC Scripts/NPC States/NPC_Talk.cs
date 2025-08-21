using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Animator interactAnim;
    public List<DialogueSO> conversations;
    public DialogueSO currentConversation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.Play("Idle");
        interactAnim.Play("Open");
    }

    private void OnDisable()
    {
        interactAnim.Play("Close");
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
                Debug.Log("Interact pressed!");

            if (GameManager.Instance.DialogueManager.isDialogueActive)
                GameManager.Instance.DialogueManager.AdvanceDialogue();
            else
            {
                if (GameManager.Instance.DialogueManager.CanStartDialogue())
                {
                    CheckForNewConversation();
                    GameManager.Instance.DialogueManager.StartDialogue(currentConversation);
                }
            }
        }
    }

    private void CheckForNewConversation()
    {
        // for (int i = conversations.Count-1; i >= 0; i++)

        for (int i = 0; i < conversations.Count; i++)
        {
            var convo = conversations[i];
            if (convo != null && convo.IsConditionMet())
            {
                currentConversation = convo;

                //remove this if its a one time only 
                if (convo.removeAfterPlay)
                    conversations.RemoveAt(i);

                //remove any other dialogue that should be cleared when this one plays(Like quest completed)
                if (convo.removeTheseOnPlay != null && convo.removeTheseOnPlay.Count > 0)
                {
                    foreach (var toRemove in convo.removeTheseOnPlay)
                    {
                        conversations.Remove(toRemove);
                    }
                }
                break;
            }
        }
    }
}
