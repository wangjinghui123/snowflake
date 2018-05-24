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
    private Vector2 _scale = Vector2.zero;

    public string entity_name = "";
    private static GameObject directionObj = null;
    private static GameObject directionObj_sprite = null;


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

    public Vector2 eulerAngles
    {
        get
        {
            return _eulerAngles;
        }
        set
        {
            _eulerAngles = value;
            if (directionObj != null)
            {
                directionObj.transform.eulerAngles = _eulerAngles;
            }
        }
    }

    private void Start()
    {
        rigidbody2D = this.GetComponent<Rigidbody2D >();
    }
    public Quaternion rotation
    {
        get
        {
            return Quaternion.Euler(_eulerAngles);
        }
        set
        {
            eulerAngles = value.eulerAngles;
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
                gameObject.transform.localScale = _scale;
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
    public void BallMove(Vector3 dir )
    {
        dir .Normalize();
        if (dir != Vector3.zero)
        {
            rigidbody2D.velocity = dir  * _speed ;
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
    public void BallDevourFood()
    {

    }


    /// <summary>
    /// 球球吐球
    /// </summary>
    public void BallSplitOutBall()
    {

    }



   


  
}
