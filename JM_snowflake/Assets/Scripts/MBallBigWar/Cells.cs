using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cells : MonoBehaviour {
    public List<GameObject> cells;
    public Sprite cellSprite;
    private int ballSpriteIndex;
    private PlayersManager playerManager;
   
    private void Awake()
    {
        playerManager = transform.parent.GetComponent<PlayersManager >();
        cells.Add(transform .GetChild (0).gameObject );
        InitBall();
    }

    public void InitBall()
    {
        ballSpriteIndex = Random.Range(0, playerManager.ballSprites.Length);
        cellSprite = playerManager.ballSprites[ballSpriteIndex];

    }

}
