using UnityEngine;

public class TotallyAccurateHammer : Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        Debug.Log(gameObject.name + "totally picked up");
    }
}
