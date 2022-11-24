using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    
    private void OnTriggerEnter (Collider other) 
	{
		//If bullet collide with "Enemy"
		if (other.transform.tag == "EnemyStomach") 
		{
            //Lower Enemy health.
			other.transform.parent.gameObject.GetComponent<EnemyHealth>().lowerHealth(50f);
			//Destroy bullet object
			Destroy(gameObject);
		}
	}

}
