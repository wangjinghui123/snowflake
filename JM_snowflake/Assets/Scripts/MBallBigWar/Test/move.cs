//using UnityEngine;
//using System.Collections;
//using System.Linq;
//public class move : MonoBehaviour
//{
//    public float playerSpeed = 0.1F;
//    private Rigidbody2D rig;
//    void Start()
//    {
//        rig = this.GetComponent<Rigidbody2D>();
//    }
//    void Update()
//    {
//        PlayerMove();

//    }
//    private void PlayerMove()
//    {
//        Vector2 vecInputValue = new Vector2(Input.GetAxis("Horizontal") * playerSpeed, Input.GetAxis("Vertical") * playerSpeed);
//        rig.AddForce(vecInputValue);
//    }




//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public ScrollCircle touch;
    public Vector3 dir;
    public void PlayerMove()
    {
        //获取horizontal 和 vertical 的值，其值位遥感的localPosition  
        float hor = touch.Horizontal;
        float ver = touch.Vertical;
        dir = new Vector3(hor, ver, 0);
        dir.Normalize();
        this.GetComponent<BallProperty>().BallMove(dir );
    }
    public void Update()
    {
        PlayerMove();
    }
}
