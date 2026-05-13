using System.Collections.Generic;

[System.Serializable]
public class Dialogue
{
    public string id;

    public string speakerName;

    public string[] sentences;
}

[System.Serializable]
public class DialogueDatabase
{
    public List<Dialogue> cutscenes;
}
