using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour {
    private float thornMass = 0;
    private Vector3 _thornScale;
    private void Start()
    {
        _thornScale = this.transform.localScale;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other .tag =="Player")
        {
            if (_thornScale.x <other.transform .localScale.x  )
            {
                //分身 体重增加 刺球消失 随机生成新的刺球

                
            }
        }
    }
}
