using System;
using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Camera cam;
    private Animator animator;

    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    private bool attacking = false;
    private bool readyToAttack = true;
    private bool blocking = false;
    
    // Blocking getter
    public bool Blocking
    {
        get => blocking;
    }

    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;

        animator = GetComponentInChildren<Animator>(true);

    }

    void Update()
    {
        
    }

    public void Attack()
    {
        if (!readyToAttack || attacking)
            return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(AttackRayCast), attackDelay);
        Invoke(nameof(ResetAttack), attackSpeed);
        
        animator.Play("Weapon_Swing");
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
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                
                Debug.Log(enemy.currentHealth);
            }
        }
    }
    
    public void Block()
    {
        blocking = true;
        
        PlayBlockAnim();
    }
    
    public void CancelBlock()
    {
        blocking = false;
        
        CancelBlockAnim();
    }
    
    // Playing blocking animation
    private void PlayBlockAnim()
    {
        if (blocking)
        {
            animator.Play("Weapon_Block_temp", 0, 0f);

            float clipLength = animator.GetCurrentAnimatorStateInfo(0).length;
            Invoke(nameof(FreezeBlockAnimation), clipLength);

        }
    }
    
    // Freezing the blocking animation at the last frame
    private void FreezeBlockAnimation()
    {
        if (blocking)
        {
            animator.speed = 0f;
        }
    }
    
    private void CancelBlockAnim()
    {
        if (!blocking)
        {
            animator.speed = 1f;
            animator.Play("Weapon_Idle");
        }
    }

    
}