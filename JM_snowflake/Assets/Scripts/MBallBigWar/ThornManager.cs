using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThornManager : MonoBehaviour {

    public RectTransform leftUpPoint;
    public RectTransform rightDownPoint;
    public Sprite[] thornSprites;
    public int maxThornBallCount;
    public GameObject thornBallPrefab;

    private void Start()
    {
        maxThornBallCount = Random.Range(2,6);
        for (int i=0;i<maxThornBallCount;i++)
        {
            SpawnThornBall();
        }
    }

    public void SpawnThornBall()
    {

        float x = Random.Range(leftUpPoint.position.x, rightDownPoint.position.x);
        float y = Random.Range(leftUpPoint.position.y, rightDownPoint.position.y);
        Vector3 thornVector3 = new Vector3(x, y, 0);
        GameObject obj = Instantiate(thornBallPrefab, this.transform);
        obj.transform.position = thornVector3;
        Image image = obj.GetComponent<Image>();
        image.sprite = thornSprites[Random.Range(0, thornSprites.Length)];
        float randomScale = Random.Range(0.4f,2f);
        obj.GetComponent<RectTransform>().localScale = new Vector3(randomScale ,randomScale ,randomScale );
    }



}
