using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<UnitScript> units = new List<UnitScript>();
    public Camera cam;
    public UnitScript selectedUnit = null;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    void OnEnable()
    {
        if (GameManager.instance != null)
        {
            Destroy(this);
        }
        else
        {
            GameManager.instance = this;
        }

        DialogueManager.DialogueAction += DisplayDialogue;
    }

    void OnDisable()
    {
        DialogueManager.DialogueAction -= DisplayDialogue;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.PositiveInfinity))
            {
                if (hit.collider.CompareTag("unit"))
                {
                    SelectUnit(hit.collider.gameObject.GetComponent<UnitScript>());
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                {
                    selectedUnit?.GoToPoint(hit.point);
                }
            }
            else
            {
                if (selectedUnit != null)
                {
                    selectedUnit.bodyRenderer.material.color = selectedUnit.unselectedColor;
                }
                selectedUnit = null;
            }
        }
    }

    public void DisplayText(string speaker, string dialogue)
    {
        nameText.text = speaker;
        dialogueText.text = dialogue;
        dialoguePanel.SetActive(true);
    }

    public void SelectUnit(UnitScript unit)
    {
        if (selectedUnit != null)
        {
            selectedUnit.bodyRenderer.material.color = selectedUnit.unselectedColor;
        }

        selectedUnit = unit;
        selectedUnit.bodyRenderer.material.color = selectedUnit.selectedColor;
    }

    public void DisplayDialogue(string characterName, string dialogue)
    {
        if (this.dialoguePanel != null && this.dialogueText != null)
        {
            dialoguePanel.SetActive(true); // Show the dialog panel
            dialogueText.text = characterName + dialogue; // Update the text
        }
        else
        {
            Debug.LogError("Dialogue panel or text is not assigned in the GameManager.");
        }
    }
}
