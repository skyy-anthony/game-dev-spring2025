using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static SimpleConditionalConversation scc;

    public static Action<string, string> DialogueAction;

    public bool useGoogleSheet = false;
    public string googleSheetDocID = "";

    void Start()
    {
        if (useGoogleSheet)
        {
            GoogleSheetSimpleConditionalConversation gs_ssc = gameObject.AddComponent<GoogleSheetSimpleConditionalConversation>();
            gs_ssc.googleSheetDocID = googleSheetDocID;
        }
        else
        {
            scc = new SimpleConditionalConversation("data");
            LoadInitialSCCState();

            // Force the state for testing the computer dialogue
            scc.setGameStateValue("clickComputer", "equals", "Clicking Computer");
            Debug.Log("Initial clickComputer state set to: " + scc.getGameStateValue("clickComputer"));
        }
    }

    public static void LoadInitialSCCState()
    {
        if (scc == null)
        {
            Debug.LogError("SCC is not initialized yet.");
            return;
        }

        Debug.Log("Checking available keys in SCC game state...");
        foreach (var key in scc.gameState.Keys)
        {
            Debug.Log("Key in game state: " + key);
        }

        if (!scc.gameState.ContainsKey("questState"))
        {
            Debug.LogWarning("questState missing. Initializing it.");
            scc.setGameStateValue("questState", "equals", "Q1T1");
        }

        scc.setGameStateValue("clickComputer", "equals", "Unclicked");
        scc.setGameStateValue("clickMonster", "equals", "Unclicked");
    }

    void Update()
    {
        if (scc == null) return;

        // SPACE: Show Boy's current line
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SPACE pressed — showing Boy's dialogue.");
            string boyLine = scc.getSCCLine("Boy");
               GameManager.instance.DisplayDialogue("Boy: ", boyLine);
        
                if (string.IsNullOrEmpty(boyLine)) boyLine = "[No Boy dialogue available]";
                     Debug.Log("Boy: " + boyLine);
        }
        {// M: Show Monster's current line}
            String monsterLine = scc.getSCCLine("Monster");
               GameManager.instance.DisplayDialogue("Monster: ", monsterLine);
 scc.setGameStateValue("clickMonster", "equals", "Clicked");
            if (string.IsNullOrEmpty(monsterLine)) monsterLine = "[No monster dialogue available]";
            
            Debug.Log("Monster: " + monsterLine);
        }
    }
}
