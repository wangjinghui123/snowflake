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
    public void PlayerMove()
    {
        //获取horizontal 和 vertical 的值，其值位遥感的localPosition  
        float hor = touch.Horizontal;
        float ver = touch.Vertical;
        Vector3 direction = new Vector3(hor, ver, 0);
        //direction.Normalize();
        //float tarangle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //if (direction != Vector3.zero)
        //{
        //    // transform.rotation = Quaternion.Slerp(transform .rotation ,Quaternion .Euler (0,0,tarangle+180 ),playerRotationSpeed *Time .deltaTime );
        //    //   transform.Translate(Vector3.left  * Time.deltaTime * playerMoveSpeed);
        //    //  transform.Translate(direction * Time.deltaTime * playerMoveSpeed);
        //    transform.GetComponent<Rigidbody2D>().velocity = direction * playerMoveSpeed;
        //}
        this.GetComponent<BallProperty>().BallMove(direction );
    }

    public void Update()
    {
        PlayerMove();
    }
}
