#define Debug
using UnityEngine;
using System.Collections;

public class StepController : MonoBehaviour
{
    //public GameObject stepOne;
    //public GameObject moviePlan;

    public GameObject stepTwo;
    public GameObject terrianMoudle;
    // public GameObject playerPrefab;

    public GameObject stepThree;

    //流程1
    public void StepOnePlay()
    {
        _StepOnePlay();
    }
    private void _StepOnePlay()
    {
        //stepOne.SetActive(true);
        stepTwo.SetActive(false);
        stepThree.SetActive(false);
    }
    //流程2
    public void StepTwoPlay()
    {
        _StepTwoPlay();
    }
    private void _StepTwoPlay()
    {
        //stepOne.SetActive(false);
        stepTwo.SetActive(true);
        stepThree.SetActive(false);
    }
    //流程3
    public void StepThreePlay()
    {
        _StepThreePlay();
    }

    private void _StepThreePlay()
    {
        //stepOne.SetActive(false);
        stepTwo.SetActive(false);
        stepThree.SetActive(true);
    }

    /// <summary>
    /// 初始化父子级关系，激活状态
    /// </summary>
    public void InitializeTheGameObj()
    {
        _InitializeTheGameObj();
    }
    private void _InitializeTheGameObj()
    {
#if Debug
        Debug.Log("Functiong  _InitializeTheGameObj() : 3D_GmaeObj Initialized");
#endif
        //流程1相关对象：父级，视频摄像机，视频面板
        //stepOne.SetActive(false);
        //moviePlan.SetActive(true);

        //流程2相关对象：父级，地形，人物模型，------------未来增加广告牌等
        stepTwo.SetActive(false);
        terrianMoudle.SetActive(true);

    }
}
