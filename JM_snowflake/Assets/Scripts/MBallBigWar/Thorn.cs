using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour {
    private float thornMass = 0;
    private Vector3 _thornScale;
    private ThornManager thornManager;
    private void Start()
    {
        _thornScale = this.transform.localScale;
        thornManager = transform.parent.GetComponent<ThornManager >();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other .tag =="Player")
        {
            if (_thornScale.x <other.transform .localScale.x  )
            {
                //分身 体重增加 刺球消失 随机生成新的刺球
                other.GetComponent<BallProperty>().BallSplit();
                thornManager.isSpawnThorn = true;
                Destroy(gameObject );

            }
        }
    }
}
