using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class AllEnemyDead : MonoBehaviour
{
    public GameObject [] enemies;
    public TMP_Text health;
    public TMP_Text playerKillCount;
    
    public int killcount;

    private void Awake()
    {
        killcount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerObj = GameObject.Find("PlayerObj");
        PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>(); 
        health.text =  "Health: " + playerHealth.checkPlayerHealth() + "";
        playerKillCount.text = "Kill Count: " + killcount + "";
    
        if((killcount >= enemies.Length))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}
