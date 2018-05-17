using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityStandardAssets.Cameras;
using System;

public class CothCollider : MonoBehaviour
{
    public Cloth cloth;
    public GameObject cube1;
    public GameObject cube2;
    public Collider myCollider;

    public Transform myParticle;

    public PlayerController playerController;

    public GameObject thelastCamera;

    void OnDisable()
    {
        isLook = false;
    }


    private bool isLook = false;
    private Transform lookGameObj;

    //TODO结尾摄像机朝向动画


    //void Update()
    //{
    //    if (isLook)
    //    {
    //        //isLook = false;
    //        ////GameObject thelastCamera = this.transform.Find("Camera").gameObject;
    //        //thelastCamera.transform.DOLookAt(lookGameObj.transform.position,0.4f).OnComplete(()=> {
    //        //    isLook = true;
    //        //});
    //        thelastCamera.transform.DOLookAt(lookGameObj.transform.position,2f);
    //    }
    //}

    private bool isCameraMove;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Debug.Log("终点线布条系统动画");
            myCollider.enabled = false;
            cloth.randomAcceleration = Vector3.down*3;
            cloth.transform.GetComponent<SkinnedMeshRenderer>().material.DOFade(0, 2f);
            Destroy(cloth.gameObject, 4f);
            if (!isCameraMove)
            {
                isCameraMove = true;
                //myParticle.gameObject.SetActive(true);
                Transform tempPlayerPos = playerController.PlayEndAnimation(thelastCamera);
                //第三相机看向指定目标
                thelastCamera.SetActive(true);
                if (tempPlayerPos == null)
                {
                    Debug.LogWarning("第三相机看向指定目标 return is null.");
                    return;
                }
                else
                {
                    Debug.LogWarning("第三相机看向指定目标 dolookat");
                    lookGameObj = tempPlayerPos;
                    isLook = true;
                    thelastCamera.transform.DOLookAt(lookGameObj.transform.TransformPoint(new Vector3(0,0,4.4f)), 3f).OnComplete(()=> {
                        thelastCamera.transform.DOLookAt(lookGameObj.transform.TransformPoint(new Vector3(0, 0, 1.4f)), 1.4f);
                    });
                }
            }

            //autoCamera.transform.SetParent(this.transform);
            //autoCamera.transform.DOLocalMove(new Vector3(2, 10, 30), 1f);
            //autoCamera.Target = this.gameObject.transform;
        }
    }
}
