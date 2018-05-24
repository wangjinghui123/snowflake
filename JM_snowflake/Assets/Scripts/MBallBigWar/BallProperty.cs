using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallProperty : MonoBehaviour
{

    public bool isPlayer = false;
    public bool isAvatar = false;
    public bool isOnDrag = false;

    private bool _isTouchBorder = false;//玩家是否触碰边界

    private Vector2 _position = Vector2.zero;
    private Vector2 _eulerAngles = Vector2.zero;
    private float _speed = 50f;
    private Vector3 _scale = new Vector3(2, 2, 2);
    private float _playerMass = 0;
    private RectTransform ballRectTransform;
    private float addMassValue = 2f;



    public ScrollCircle scrollCircle;

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
    private void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D>();
        ballRectTransform = this.GetComponent<RectTransform>();
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


    /// <summary>
    /// 球球移动
    /// </summary>
    /// <param name="移动方向"></param>
    public void BallMove(Vector3 dir)
    {
        dir.Normalize();
        if (dir != Vector3.zero)
        {
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
    /// 球球吞食食物
    /// </summary>
    /// <param name="mass"></param>
    /// <param name="addScaleValue"></param>
    public void BallDevourFood(int mass, float addScaleValue)
    {
        _playerMass += mass;

        ////addScaleValue = Mathf.Log(2,_scale.x   );
        //if (_scale.x <= 2)
        //{
        //    _scale += new Vector3(addScaleValue, addScaleValue, addScaleValue);
        //    //_scale = new Vector3(addScaleValue ,addScaleValue,addScaleValue ); 
        //    ballRectTransform.localScale = _scale;
        //}
        //else
        //{

        //    addScaleValue = Mathf.Log(_scale .x,2 );
        //    _scale = new Vector3(addScaleValue ,addScaleValue ,addScaleValue );
        //    ballRectTransform.localScale = _scale;
        //}

        addScaleValue++;
        float y = Mathf.Log(addMassValue, 2);
        ballRectTransform.localScale = _scale;
    }
    /// <summary>
    /// 球球吐球
    /// </summary>
    public void BallSplitOutBall()
    {

    }







}
