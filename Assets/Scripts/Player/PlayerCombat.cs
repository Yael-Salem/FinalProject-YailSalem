using System;
using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    private Camera cam;
    private Animator animator;
    private Coroutine blockRoutine;

    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 10;
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
        if (!readyToAttack || attacking || Time.timeScale == 0f)
            return;
        
        // Variable to store whether the enemy we are currently fighting dodged our attack or not
        bool attackDodged = false;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance,
                attackLayer))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();

            if (enemy != null)
                attackDodged = enemy.TryDodge();
        }

        readyToAttack = false;
        attacking = true;

        // Calling on the raycast only if the enemy did not dodge our attack
        if(!attackDodged)
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
            EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>(); // Reference to enemy's health

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
                
                Debug.Log(enemyHealth.currentHealth);
            }
        }
    }
    
    public void Block()
    {
        if (Time.timeScale == 0f)
            return;
        
        blocking = true;
        
        PlayBlockAnim();
    }
    
    public void CancelBlock()
    {
        blocking = false;
        
        if(blockRoutine != null)
            StopCoroutine(blockRoutine);

        blockRoutine = null;
        
        CancelBlockAnim();
    }
    
    // Playing blocking animation
    private void PlayBlockAnim()
    {
        if (blocking)
        {
            animator.speed = 1f;
            animator.Play("Weapon_Block", 0, 0f);
            
            if(blockRoutine != null)
                StopCoroutine(blockRoutine);

            blockRoutine = StartCoroutine(FreezeBlockAtEnd());

        }
    }

    private IEnumerator FreezeBlockAtEnd()
    {
        yield return null; // Wait a frame so Play() takes effect

        while (blocking)
        {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

            if (info.IsName("Weapon_Block") && info.normalizedTime >= 1f)
            {
                animator.speed = 0f;
                yield break;
            }
            
            yield return null;
        }
    }

    // Freezing the blocking animation at the last frame
    public void FreezeBlockAnimation()
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