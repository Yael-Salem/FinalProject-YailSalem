using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Camera cam;
    
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    private bool attacking = false;
    private bool readyToAttack = true;
    private int attackCount;

    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
    }
	
    public void Attack()
    {
        if (!readyToAttack || attacking)
            return;

        readyToAttack = false;
        attacking = true;
        
        Invoke(nameof(AttackRayCast), attackDelay);
        Invoke(nameof(ResetAttack), attackSpeed);
        
    }

    public void ResetAttack()
    {
        
        attacking = false;
        readyToAttack = true;
    }

    public void AttackRayCast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance,
                attackLayer))
        {
            Debug.Log("Object hit");
            Destroy(hit.transform.gameObject);
        }
    }

}
