using System;
using UnityEngine;

public class WeaponPickUp : Interactable
{
    public GameObject WeaponOnPlayer;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WeaponOnPlayer.SetActive(false);
    }

    protected override void Interact()
    {
        this.gameObject.SetActive(false);
        
        WeaponOnPlayer.SetActive(true);
        
        Debug.Log(gameObject.name + " picked up");
    }
}
