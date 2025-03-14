using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor.Rendering;
#endif
using UnityEngine;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    [SerializeField] List<DialogueNode> nodes = new();
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI leftName;
    [SerializeField] TextMeshProUGUI rightName;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject leftPanel;
    [SerializeField] GameObject rightPanel;
    [SerializeField] Image leftPortrait;
    [SerializeField] Image rightPortrait;
    bool playerPressed = false;
    private bool isActive = false;
    private int currentIndex = 0;

    void Start()
    {

    }

    void Update()
    {
        if (isActive && playerPressed) // If dialogue is active and player presses a key
        {
            playerPressed = false; // Reset input flag
            currentIndex++; // Move to the next dialogue line
            RunDialogue(); // Show the next line
        }
    }
    public void StartDialogue(List<DialogueNode> newNodes)
    {
        nodes = newNodes; // Set new dialogue nodes
        currentIndex = 0; // Reset index
        isActive = true;
        RunDialogue(); // Start dialogue
    }


    // Update is called once per frame
    public void PlayerPressed()
    {
        playerPressed = true;
    }

    void RunDialogue()
    {
        if (currentIndex >= nodes.Count) // If no more dialogue left, hide the panel
        {
            dialoguePanel.SetActive(false);
            leftPanel.SetActive(false);
            rightPanel.SetActive(false);
            isActive = false;
            return;
        }

        isActive = true;
        dialoguePanel.SetActive(true);

        DialogueNode node = nodes[currentIndex]; // Get the current dialogue node

        leftPanel.SetActive(node.isLeft);
        rightPanel.SetActive(!node.isLeft);

        if (node.isLeft)
        {
            leftName.text = node.talkerName;
            leftPortrait.sprite = node.portrait;
        }
        else
        {
            rightName.text = node.talkerName;
            rightPortrait.sprite = node.portrait;
            rightPortrait.rectTransform.localScale = new Vector3(-1, 1, 1); // Flipped
        }

        dialogueText.text = node.dialogueText;
    }
    public bool GetStatus()
    {
        return isActive;
    }
}
