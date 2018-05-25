using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallProperty : MonoBehaviour
{
    public float _maxSpeed = 50;
    public float _minSpeed = 10;


    private Vector2 _position = Vector2.zero;
    private Vector2 _eulerAngles = Vector2.zero;
    private float _speed = 50f;
    private Vector3 _scale = new Vector3(0.3f,0.3f,0.3f);
    private float _playerMass = 0;
    private RectTransform ballRectTransform;//玩家自身坐标
    private float addMassValue = 2f;//
    private int ballSpriteIndex;


    private PlayersManager playerManager;
    private Rigidbody2D rigidbody2D;
    public Vector2 position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            if (gameObject != null)
            {
                gameObject.transform.position = _position;
            }
        }
    }


    public float playerMass
    {
        get
        {
            return _playerMass;
        }
        set
        {
            _playerMass = value;
        }
    }
   
    public Vector2 scale
    {
        get
        {
            return _scale;
        }
        set
        {
            _scale = value;
            if (gameObject != null)
            {
                //   gameObject.transform.localScale = _scale;
                ballRectTransform.localScale = _scale;
            }
        }
    }
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }


    private void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        ballRectTransform = this.GetComponent<RectTransform>();
        playerManager = this.transform.parent.GetComponent<PlayersManager>();
        InitBall();
    }

    private void Update()
    {
        if (Input .GetKey (KeyCode .Q ))
        {
            BallSplitOutBall();
        }
       
    }
    

    public void InitBall()
    {
        ballSpriteIndex = Random.Range(0,playerManager.ballSprites .Length );
        this.GetComponent<Image>().sprite = playerManager.ballSprites[ballSpriteIndex ];

    }

    /// <summary>
    /// 球球移动
    /// </summary>
    /// <param name="移动方向"></param>
    public void BallMove(Vector3 dir)
    {
        dir.Normalize();
        if (dir != Vector3.zero)
        {
            _speed = Mathf.Clamp((1 / ballRectTransform.localScale.x * 50), 5, 60);
            rigidbody2D.velocity = dir * _speed;
        }
    }

    /// <summary>
    /// 球球分身
    /// </summary>
    public void BallSplit()
    {

    }

    /// <summary>
    /// 球球吞食食物 增加质量 增加Scale
    /// </summary>
    /// <param name="增加质量值"></param>
    public void BallDevourFood(float mass)
    {

        _playerMass += mass;
        float y = Mathf.Log(addMassValue, 8f);
        _scale = new Vector3(y, y, y);
        ballRectTransform.localScale = _scale;
        addMassValue++;
      //  Debug.Log("=====================================" + _playerMass + "===============================");
    }

    /// <summary>
    /// 球球吐球
    /// </summary>
    public void BallSplitOutBall()
    {
        
    }
}
