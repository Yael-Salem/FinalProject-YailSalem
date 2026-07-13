using System.Collections.Generic;

[System.Serializable]
public class ObjectiveItem
{
    public string id; // The objective's ID stored in the objectives.json file
    public string title; // The actual text the player sees for each objective
}

[System.Serializable]
public class ObjectiveDatabase
{
    public List<ObjectiveItem> objectives;
}