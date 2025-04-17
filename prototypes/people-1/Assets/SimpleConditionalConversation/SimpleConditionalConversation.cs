using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleConditionalConversation 
{
    public Dictionary<string, object> gameState;
    public string questState = "Q1T1";

    Hashtable lines;
    
    public SimpleConditionalConversation(string dataPath)
    {
        this.gameState = new Dictionary<string, object>();
        List<Dictionary<string, object>> data = CSVReader.Read(dataPath);
        this.loadLines(data);
    }
    
    public SimpleConditionalConversation(List<Dictionary<string, object>> data)
    {
        this.gameState = new Dictionary<string, object>();
        this.loadLines(data);
    }
    
    public void loadLines(List<Dictionary<string, object>> data) 
    {
        this.lines = new Hashtable();
    
        for (var i = 0; i < data.Count; i++) {
            if (!lines.ContainsKey((string)data[i]["questState"])) {
                lines.Add((string)data[i]["questState"], new Dictionary<string, List<SCCLine>>());
            }
            Dictionary<string, List<SCCLine>> questStateLines = (Dictionary<string, List<SCCLine>>)lines[(string)data[i]["questState"]];
            if (!questStateLines.ContainsKey((string)data[i]["character"])) {
                questStateLines[(string)data[i]["character"]] = new List<SCCLine>();
            }
            List<SCCLine> characterLines = questStateLines[(string)data[i]["character"]];
            SCCLine line = new SCCLine();
            line.questState = (string)data[i]["questState"];
            line.character = (string)data[i]["character"];
            line.condition1Left = (string)data[i]["condition1Left"];
            line.condition1Comp = (string)data[i]["condition1Comp"];
            line.condition1Right = data[i]["condition1Right"];
            line.condition2Left = (string)data[i]["condition2Left"];
            line.condition2Comp = (string)data[i]["condition2Comp"];
            line.condition2Right = data[i]["condition2Right"];
            line.effectLeft = (string)data[i]["effectLeft"];
            line.effectOp = (string)data[i]["effectOp"];
            line.effectRight = data[i]["effectRight"];
            line.line1 = (string)data[i]["line1"];
            line.line2 = (string)data[i]["line2"];
            line.line3 = (string)data[i]["line3"];
            characterLines.Add(line);
        }

        foreach (var key in lines.Keys)
        {
            Debug.Log("QuestState in lines: " + key);
        }
    }

    public string getSCCLine(string name)
    {
        Debug.Log($"Retrieving dialogue for character: {name}");
        if (!lines.ContainsKey(questState))
        {
            Debug.LogError($"Quest state '{questState}' not found in lines.");
            return "No dialogue available.";
        }

        Dictionary<string, List<SCCLine>> questLines = (Dictionary<string, List<SCCLine>>)lines[questState];
        foreach (var character in questLines.Keys)
        {
            Debug.Log($"Character in questLines: {character}");
        }

        if (!questLines.ContainsKey(name))
        {
            Debug.LogError($"Character {name} not found in quest lines.");
            return "No dialogue available.";
        }

        List<SCCLine> characterLines = questLines[name];
        foreach (SCCLine line in characterLines)
        {
            if (checkCondition(line.condition1Left, line.condition1Comp, line.condition1Right) &&
                checkCondition(line.condition2Left, line.condition2Comp, line.condition2Right))
            {
                string dialogueLine = line.renderLine();
                Debug.Log($"Retrieved line: {dialogueLine}");
                return dialogueLine;
            }
        }

        return "No valid dialogue found.";
    }

    public bool checkCondition(string left, string op, object right) 
    {
        object leftValue = getGameStateValue(left);

        if (op == "not equals" && leftValue == null) {
            return true;
        } else if (leftValue == null && op != "") {
            return false;
        }

        if (leftValue is int) {
            int leftInt = (int)leftValue;
            if (op == "greater") {
                return leftInt > (int)right;
            } else if (op == "less") {
                return leftInt < (int)right;
            } else if (op == "equals") {
                return leftInt == (int)right;
            } else if (op == "not equals") {
                return leftInt != (int)right;
            }
        } else if (leftValue is float) {
            float leftFloat = (float)leftValue;
            float rightFloat;
            float.TryParse(right.ToString(), out rightFloat);
            if (op == "greater") {
                return leftFloat > rightFloat;
            } else if (op == "less") {
                return leftFloat < rightFloat;
            } else if (op == "equals") {
                int leftInt = (int)leftFloat;
                int rightInt = (int)right;
                return leftInt == rightInt;
            } else if (op == "not equals") {
                int leftInt = (int)leftFloat;
                int rightInt = (int)right;
                return leftInt != rightInt;
            }
        } else if (leftValue is bool) {
            bool leftBool = (bool)leftValue;
            right = right.ToString().ToLower();
            right = bool.Parse((string)right);
            if (op == "equals") {
                return leftBool == (bool)right;
            } else if (op == "not equals") {
                return leftBool != (bool)right;
            }
        } else {
            if (op == "equals") {
                return (string)leftValue == (string)right;
            } else if (op == "not equals") {
                if (leftValue == null || (string)leftValue != (string)right) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        return true;
    }

    public object getGameStateValue(string id) 
    {
        if (this.gameState.ContainsKey(id)) {
            return this.gameState[id];
        }
        return null;
    }

    public void setGameStateValue(string id, string op, object right) 
    {
        if (op == "add") {
            if (!this.gameState.ContainsKey(id)) {
                this.gameState.Add(id, 0);
            }
            this.gameState[id] = (int)this.gameState[id] + (int)right;
        } else if (op == "subtract") {
            if (!this.gameState.ContainsKey(id)) {
                this.gameState.Add(id, 0);
            }
            this.gameState[id] = (int)this.gameState[id] - (int)right;
        } else if (op == "equals" || op == "set") {
            bool rightBool;
            float rightFloat;
            if (float.TryParse(right.ToString(), out rightFloat)) {
                if (!this.gameState.ContainsKey(id)) {
                    this.gameState.Add(id, right);
                } else {
                    this.gameState[id] = right;
                }
            } else if (bool.TryParse((string)right, out rightBool)) {
                if (!this.gameState.ContainsKey(id)) {
                    this.gameState.Add(id, rightBool);
                } else {
                    this.gameState[id] = rightBool;
                }
            } else {
                if (!this.gameState.ContainsKey(id)) {
                    this.gameState.Add(id, (string)right);
                } else {
                    this.gameState[id] = (string)right;
                }
            }
        }
    }
}
