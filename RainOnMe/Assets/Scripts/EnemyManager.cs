using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    // Stats
    public float maxHealth = 100f;
    private float currentHealth;

    // attacking vars
    public Transform basicAttackPoint;
    public float basicAttackDamage;
    public float basicAttackRange;
    public LayerMask playerLayer;
    private bool canAttack;
    public float basicAttackCooldownValue;
    public float attackAnimationDelayValue;
    private bool canTakeDamage = true;
    public float takeDamageCooldownValue;
    public float deathAnimationDelay;

    // animator vars
    private Animator animator;

    public Image healthbar;

    private void Awake()
    {
        //currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage && currentHealth != 0)
        {
            canTakeDamage = false;
            animator.SetTrigger("Hit");
            currentHealth -= damage;
            healthbar.fillAmount = currentHealth / maxHealth;
            if (currentHealth <= 0)
            {
                Death();
            }
            StartCoroutine(TakeDamageDelay());
        }

    }

    public  void BasicAttack()
    {

    }

    private void Death()
    {
        canTakeDamage = false;
        animator.SetBool("Death", true);
        StartCoroutine(DeathAnimationDelay());
    }

    IEnumerator TakeDamageDelay()
    {
        yield return new WaitForSeconds(takeDamageCooldownValue);
        canTakeDamage = true;
    }

    IEnumerator DeathAnimationDelay()
    {
        yield return new WaitForSeconds(deathAnimationDelay);
        //animator.SetBool("Death", false);

        GetComponent<BoxCollider2D>().enabled = false;  
        this.enabled = false;

        //this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           // Debug.Log("Player Hit");
        }
    }
}
