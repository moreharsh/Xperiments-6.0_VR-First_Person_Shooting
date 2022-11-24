using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

	[SerializeField]
	private float health = 100f;
	// Use this for initialization

	private void isPlayerAlive()
	{
		if(health <= 0)
		{
			Debug.Log("QIUT");
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
	}

	public float checkPlayerHealth()
	{
		return health;	
	}

    public void lowerPlayerHealth(float amount)
    {
		if(health-amount <= 0)
        {
			health = 0;
		}
        else
        {
			health -= amount;
		}
		
		isPlayerAlive();
    }
}
