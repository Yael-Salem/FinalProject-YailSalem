using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    public PlayerCombat playerCombat;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerCombat == null)
            playerCombat = GetComponentInParent<PlayerCombat>();
    }

    public void onLastFrameReached()
    {
        if(playerCombat != null)
            playerCombat.FreezeBlockAnimation();
    }
}
