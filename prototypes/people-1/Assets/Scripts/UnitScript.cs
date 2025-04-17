using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitScript : MonoBehaviour
{
    public NavMeshAgent nma;

    // The below are all of the stuff that populates the UI (and makes the units
    // individual).
    public string characterName;

    public Renderer bodyRenderer;

    public Color unselectedColor;
    public Color selectedColor;

    private UnitState currentState;

    public GameObject computerSphere; // Reference to the computerSphere GameObject
    private bool isVisible = false; // Track visibility state

    private void OnEnable()
    {
        // Initialize state
        ChangeState(new IdleState(this));
    }

    private void OnDisable()
    {
        // Clean up state
        ChangeState(new IdleState(this));
    }

    // Start is called before the first frame update
    void Start()
    {
        nma = gameObject.GetComponent<NavMeshAgent>();
        unselectedColor = bodyRenderer.material.color;
        GameManager.instance.units.Add(this);

        // Make the GameObject invisible at the start
        SetVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Update();

        // Check for interaction with the computerSphere
        if (computerSphere != null && Input.GetKeyDown(KeyCode.E)) // Example: Press 'E' to interact
        {
            float distance = Vector3.Distance(transform.position, computerSphere.transform.position);
            if (distance <= 3f) // Example interaction range
            {
                TriggerComputerDialogue(); // Trigger the computer's dialogue
            }
        }
    }

    public void GoToPoint(Vector3 point)
    {
        nma.SetDestination(point);
        ChangeState(new WalkingState(this));
    }

    public void ChangeState(UnitState newState)
    {
        if (currentState == newState) return;

        // Handle state exit
        currentState?.Exit();

        // Change to new state
        currentState = newState;

        // Handle state enter
        currentState?.Enter();
    }

    public UnitState GetCurrentState()
    {
        return currentState;
    }

    private void OnDestroy()
    {
        GameManager.instance.units.Remove(this);
    }

    private void SetVisibility(bool visible)
    {
        isVisible = visible;
        bodyRenderer.enabled = visible; // Toggle the Renderer visibility
        nma.enabled = visible; // Enable or disable the NavMeshAgent
    }

    private void TriggerComputerDialogue()
    {
        Debug.Log("Interacting with the computer...");

        // Trigger the computer's dialogue using the DialogueManager
        if (DialogueManager.scc != null)
        {
            string line = DialogueManager.scc.getSCCLine("Computer");
            GameManager.instance.DisplayDialogue("Computer: ", line);
            Debug.Log("Computer says: " + line);

            // Optionally update the game state
            DialogueManager.scc.setGameStateValue((string)"clickComputer", "equals", (object)"Clicked");
            Debug.Log("Game state updated: clickComputer = " + DialogueManager.scc.getGameStateValue("clickComputer"));
        }
        else
        {
            Debug.LogError("DialogueManager or SCC is not initialized.");
        }
    }
}
