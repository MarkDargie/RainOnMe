using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D boxCollider;

    //Stats
    public float maxHealth = 3;
    public float currentHealth;
    private float playerAttackSpeedModifier;

    // Attacking
    public float basicAttackCooldownValue;
    public float attackAnimationDelayValue;
    private bool canAttack = true;
    public bool isAttacking = false;
    public Transform basicAttackPoint; 
    public float basicAttackRange;
    public float basicAttackDamage;
    public LayerMask enemyLayers;
    private int airAttackCount = 0;


    private RigidbodyConstraints2D originalConstraints;

    private PlayerMovement playerMovement;
    private bool isDashing;
    private bool isGrounded;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalConstraints = rb.constraints;
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isDashing = playerMovement.isDashing;
        isGrounded = playerMovement.isGrounded;

        if (isGrounded) airAttackCount = 0;

        // Left click basic attack
        if (Input.GetMouseButtonDown(0) && canAttack && !isDashing && airAttackCount < 1)
        {
            BasicAttack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }
        //Right Click Strong/shoot? Attack

    }

    // Player Basic Attack 
    private void BasicAttack()
    {
        isAttacking = true;

        if(!isGrounded) airAttackCount++;

        animator.SetTrigger("Attack");
        StartCoroutine(AttackAnimationDelay());
        canAttack = false;
        StartCoroutine(InstantiateBasicAttackDamage());
        StartCoroutine(BasicAttackCooldown()); 

    }

    public void TakeDamage(float damage)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damage;
        }
        else
        {
            // die
        }
    }

    private void Die()
    {

    }

    // Delay before attacking
    IEnumerator InstantiateBasicAttackDamage()
    {
        yield return new WaitForSeconds(0.2f);
        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, enemyLayers);
        foreach (Collider2D target in targetsHit)
        {
            target.GetComponent<EnemyCombatManager>().TakeDamage(basicAttackDamage);
        }
        //isAttacking = false;
    }

    // Set Cooldown for left click basic attack
    IEnumerator BasicAttackCooldown()
    {
        yield return new WaitForSeconds(basicAttackCooldownValue);
        canAttack = true;
    }

    // disable movemnt on Y axis during attack (remain airborne if not grounded)
    IEnumerator AttackAnimationDelay()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(attackAnimationDelayValue);
        rb.constraints = originalConstraints;
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision", collision.gameObject);
        if (collision.gameObject.tag == "EnemyHitbox")
        {
            Debug.Log("SUS IMPOSTER");
        }
    }

   //void OnTriggerEnter2D(Collider2D other)
   // {
   //     if (other.gameObject.tag == "EnemyHitBox")a
   //     {
   //         Debug.Log("BiG CHUNGUS");
   //     }
   // }

    // Dislay attack range in scene view
    private void OnDrawGizmosSelected()
    {
        if (basicAttackPoint == null) return;
        Gizmos.DrawWireSphere(basicAttackPoint.position, basicAttackRange);
    }

}
