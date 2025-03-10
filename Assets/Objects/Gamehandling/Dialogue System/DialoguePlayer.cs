using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] List<DialogueNode> nodes = new();
    [SerializeField] Text dialogueText;
    [SerializeField] Text leftName;
    [SerializeField] Text rightName;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject leftPanel;
    [SerializeField] GameObject rightPanel;
    [SerializeField] Image leftPortrait;
    [SerializeField] Image rightPortrait;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RunDialogue()
    {

    }
}
