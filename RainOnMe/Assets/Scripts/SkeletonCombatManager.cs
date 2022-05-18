using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCombatManager : MonoBehaviour
{

    private EnemyManager enemyManager;
    private Animator animator;

    [Header("Combat Stats")]
    public float basicAttackDamage;
    public float basicAttackRange;
    public float basicAttackCooldown;
    private bool canAttack;
    private Transform basicAttackPoint;


    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();    
        animator = GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void BasicAttack()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
