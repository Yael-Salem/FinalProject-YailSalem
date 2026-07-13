using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string id;

    public List<DialogueLine> lines;

    public string triggersObjectiveId;
}

[System.Serializable]
public class DialogueDatabase
{
    public List<Dialogue> cutscenes;
}

[System.Serializable]
public class DialogueLine
{
    public string speaker;

    public string text;

    public string lookTargetTag; // In case we need the player to look a certain direction
}