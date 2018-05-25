using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFoodTrigger : MonoBehaviour {

    private FoodManager foodManager;
    private  float _starFoodMass;
    public float maxStarFoodMass = 100;
    public float minStarFoodMass = 8;
    public  float starFoodMass
    {
        get
        {
            return _starFoodMass;
        }
        set
        {
            _starFoodMass = value;
        }
    }

   
    private void Start()
    {
        foodManager = this.transform.parent.GetComponent<FoodManager>();
        _starFoodMass = Random.Range(minStarFoodMass ,maxStarFoodMass );

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other .tag =="Player")
        {
            other.gameObject.GetComponent<BallProperty>().BallDevourFood(_starFoodMass );
            transform.parent.GetComponent<FoodManager>().MoveFoodPos(gameObject );
        }
    }
}
