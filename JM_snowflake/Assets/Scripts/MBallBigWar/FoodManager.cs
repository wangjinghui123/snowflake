using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodManager : MonoBehaviour
{


    public Color[] colors;
    public Sprite[] foodSprites;
    public List<GameObject> foodObjList = new List<GameObject>();
    public int _maxFoodCount = 100;

    private RectTransform foodManagerRect;
    public RectTransform LeftUpPoint;
    public RectTransform RightDownPoint;
    public GameObject starFoodPrefab;

    public int maxFoodMass
    {
        get
        {
            return _maxFoodCount;
        }
        set
        {
            _maxFoodCount = value;
        }
    }

    private void Start()
    {
        foodManagerRect = this.GetComponent<RectTransform>();
        for (int i = 0; i < _maxFoodCount; i++)
        {
            foodObjList.Add(SpawnFood());

        }
    }



    public GameObject SpawnFood()
    {
        if (foodObjList.Count <= _maxFoodCount)
        {
            //float x = Random.Range(LeftUpPoint.position.x, RightDownPoint.position.x);
            //float y = Random.Range(LeftUpPoint.position.y, RightDownPoint.position.y);
            //Vector3 foodVector3 = new Vector3(x, y, 0);
            GameObject obj = Instantiate(starFoodPrefab, this.transform);
            obj.transform.position = RandomGamePos();
            Image image = obj.GetComponent<Image>();
            image.color = colors[Random.Range(0, colors.Length - 1)];
            image.sprite = foodSprites[Random.Range(0, foodSprites.Length)];

            return obj;

        }
        else
            return null;
    }

    public Vector3 RandomGamePos()
    {
        float x = Random.Range(LeftUpPoint.position.x, RightDownPoint.position.x);
        float y = Random.Range(LeftUpPoint.position.y, RightDownPoint.position.y);
        Vector3 foodVector3 = new Vector3(x, y, 0);
        return foodVector3;
    }
    public void MoveFoodPos(GameObject food)
    {
        food.transform.position = RandomGamePos();
        food.GetComponent<Image>().color = colors[Random.Range(0, colors.Length)];
    }
}
