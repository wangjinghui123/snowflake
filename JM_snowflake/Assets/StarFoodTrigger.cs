using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFoodTrigger : MonoBehaviour {

    private FoodManager foodManager;

    private void Start()
    {
        foodManager = this.transform.parent.GetComponent<FoodManager>();

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("666");
        if (other .tag =="Player")
        {
           
            other.gameObject.GetComponent<BallProperty>().BallDevourFood(1,0.05f);
            Destroy(gameObject );

        }
    }
}
