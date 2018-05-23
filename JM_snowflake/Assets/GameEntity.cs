using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEntity : MonoBehaviour
{

    public bool isPlayer = false;
    public bool isAvatar = false;

    private bool _isTouchBorder = false;//玩家是否触碰边界

    private Vector2 _position = Vector2.zero;
    private Vector2 _eulerAngles = Vector2.zero;
    private float _speed = 0f;
    private Vector2 _scale = Vector2.zero;

    public string entity_name = "";
    private static GameObject directionObj = null;
    private static GameObject directionObj_sprite = null;

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


    public void UpdatePosition()
    {
        this.GetComponent<move>().PlayerMove();
        if (!_isTouchBorder)
        {

        }

    }


    private void Update()
    {
        UpdatePosition();
    }
    public bool TestPos()
    {
        if (transform .position.x)
        {

        }
        return _isTouchBorder;
    }
}
