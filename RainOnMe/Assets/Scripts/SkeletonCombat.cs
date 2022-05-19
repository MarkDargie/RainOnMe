using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCombat : MonoBehaviour
{

    private EnemyManager enemyManager;
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Combat Stats")]
    public float basicAttackDamage;
    public float basicAttackRange;
    public float basicAttackCooldown;
    public float basicAttackDelay;
    public float playerDetectionRange;
    public float attackAnimationDelay;

    private bool canAttack = true;
    public Transform basicAttackPoint;
    public LayerMask playerLayers;


    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();    
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", rb.velocity.x);

        // check player is in range
        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(gameObject.transform.position, playerDetectionRange, playerLayers);
        foreach(Collider2D player in playerInRange)
        {
            if(canAttack)
            {
                BasicAttack();
            }
        }

    }

    private void BasicAttack()
    {
        // lock rotation
        canAttack = false;
        Debug.Log("Basic Attack");
        StartCoroutine(InstantiateBasicAttackDamage());
    }

    IEnumerator InstantiateBasicAttackDamage()
    {
        yield return new WaitForSeconds(attackAnimationDelay);
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(basicAttackDelay);
        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, playerLayers);
        foreach(Collider2D target in targetsHit)
        {
            Debug.Log("Player Hit");
            target.GetComponent<PlayerCombat>().TakeDamage(basicAttackDamage);
        }
        //animator.ResetTrigger("Attack");
        StartCoroutine(BasicAttackCooldown());
    }

    IEnumerator BasicAttackCooldown()
    {
        Debug.Log("Basic Attack Cooldown: " + canAttack);
        yield return new WaitForSeconds(basicAttackCooldown);
        canAttack = true;
        Debug.Log("Basic Attack Cooldown: " + canAttack);
    }

    // draw player detection and basic attack ranges 
    private void OnDrawGizmosSelected()
    {
        if (playerDetectionRange == null) return;
        Gizmos.DrawWireSphere(gameObject.transform.position, playerDetectionRange);

        if(basicAttackRange == null) return;
        Gizmos.DrawWireSphere(basicAttackPoint.position, basicAttackRange);
    }

}
