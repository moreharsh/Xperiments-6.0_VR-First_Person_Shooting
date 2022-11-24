using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    Animator animator;
    public float health = 100f;
    private float maxHealth = 100f;
    bool isDead = false;
    
    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
    }
    public void lowerHealth(float amount)
    {
        health -= amount;
        CheckEnemyHealth();
    }

    private void CheckEnemyHealth()
    {
        if(health <= 0f && !isDead)
        {
            animator.SetTrigger("Die");
            
            GameObject playerObj = GameObject.Find("PlayerObj");
            AllEnemyDead allEnemyDead = playerObj.GetComponent<AllEnemyDead>();
            allEnemyDead.killcount = allEnemyDead.killcount + 1;

            gameObject.GetComponent<EnemyAI>().getEnemyDead();
            Invoke(nameof(EnemyDie), 10f);
            isDead = true;
        }
    }

    private void EnemyDie()
    {
        Destroy(gameObject);
    }

}
