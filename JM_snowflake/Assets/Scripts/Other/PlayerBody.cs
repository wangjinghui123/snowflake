//#define ISCAR

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

public class PlayerBody : MonoBehaviour
{
    public modleType modelType;
    public string isNpc { get { return ((int)_isNpc).ToString(); } set { _isNpc = (InitiaItIsNpc)int.Parse(value); } }
    public InitiaItIsNpc _isNpc = InitiaItIsNpc.no;
    public int startTime = 0;
    public int endTime = 0;
    public Transform[] wheels;


    public GameObject boxFbx
    {
        get
        {
            return _boxFbx;
        }
    }
    public GameObject TargetCamera
    {
        get
        {
            return targetCamera;
        }
        set
        {
            targetCamera = value;
        }
    }

    [SerializeField]
    private GameObject targetCamera;
    public GameObject _headPortriatObj;
    [SerializeField]
    private GameObject _headPotriatMap;
    [SerializeField]
    private GameObject _boxFbx;

    private Vector3 moveTarget = Vector3.zero;

    public int num_Animator = 0;
    public Animator animator
    {
        get
        {
            return _animator;
        }
        set
        {
            _animator = value;
        }
    }
    private Animator _animator;

    public userStatus status = userStatus.defult;

    /// <summary>
    ///共有属性：玩家昵称
    /// </summary>
    public string nickName
    {
        get
        {
            return _nickName;
        }
        set
        {
            _nickName = value;
        }
    }
    private string _nickName;
    /// <summary>
    ///共有属性：玩家ID
    /// </summary>
    public string userId
    {
        get
        {
            return _userId;
        }
        set
        {
            _userId = value;
        }
    }
    private string _userId;

    /// <summary>
    /// 用户头像
    /// </summary>
    public HeadPortriat headPortriat
    {
        get
        {
            if (_heaPortrait == null)
            {
                //Debug.Log("Property headPortriat.get(): _headPortriat == null. ");
                _heaPortrait = new HeadPortriat();
            }
            return _heaPortrait;
        }
        set
        {
            if (_heaPortrait == null)
            {
                //Debug.Log("Property headPortriat.set(): _headPortriat == null. ");
                _heaPortrait = new HeadPortriat();
            }
            headPortriat = value;
        }
    }
    private HeadPortriat _heaPortrait;
    /// <summary>
    ///共有属性：步数设置
    /// </summary>
    public int score
    {
        get
        {
            return (int)(_score);//+ _hisScore);
        }
        set
        {
            _score = value;
            //_hisScore += _score * 3f;
            //_hisScore *= 0.3f;
        }
    }
    private int _score = 0;
    //public float _hisScore = 0;
    //赛道标记
    public int trackeTag
    {
        get
        {
            return _trackTag;
        }
        set
        {
            _trackTag = value;
        }
    }
    [SerializeField]
    private int _trackTag = 0;

    //排名标识
    public int lastRankTag
    {
        get
        {
            return _lastRankTag;
        }
        set
        {
            _lastRankTag = value;
        }
    }
    public int currentRankTag
    {
        get
        {
            return _currentRankTag;
        }
        set
        {
            _currentRankTag = value;
        }
    }
    [SerializeField]
    private int _lastRankTag;
    [SerializeField]
    private int _currentRankTag;

    public GameObject playerParent;


#if ISCAR
    private float _leftSizex = -3f;
    private float _xSpan = 1.2f;
#else
    private float _leftSizex = -2.25f;
    private float _xSpan = 0.5f;
#endif




    void Start()
    {
        GetTheHeadPortriat();

    }
    void OnEnable()
    {
#if ISCAR
        if (_currentRankTag < 4)

#else
        _animator = this.GetComponent<Animator>();
        _animator.speed = UnityEngine.Random.Range(0.8f, 1.3f);
            if (_currentRankTag < 8)
#endif
            this.transform.localPosition = new Vector3(_leftSizex + _trackTag * _xSpan, -0.4f, 0f);
        else
            this.transform.localPosition = new Vector3(0, -0.4f, -5f);
        moveTarget = this.transform.localPosition;
    }
    void Update()
    {
        _headPortriatObj.transform.parent.transform.LookAt(targetCamera.transform);
        _headPotriatMap.transform.forward = -Vector3.right;

#if ISCAR

#else
        _animator.SetInteger("Status", num_Animator);
#endif
        _UpdatePosition();
    }


    #region 用户头像下载
    /// <summary>
    /// 获取用户头像
    /// </summary>
    //public void GetTheHeadPortriat()
    //{
    //    if (_heaPortrat != null)
    //        if (_heaPortrat.headPortraitTexture == null && !string.IsNullOrEmpty(_heaPortrat.headPortraitUrl))
    //            _GetTheHeadPortriat();
    //        else
    //            Debug.LogWarning("图片已经下载完成，重复调用!!!!!");
    //}
    //int _restartCount = 0;
    //private void _GetTheHeadPortriat()
    //{
    //    if (string.IsNullOrEmpty(_heaPortrat.headPortraitUrl))
    //    {
    //        Debug.Log("Fuction _GetTheHeadPortriat(): The head portriat url is null.");
    //        return;
    //    }
    //    StartCoroutine(_GetHeadPortriatIEnumer(_heaPortrat.headPortraitUrl));
    //}
    //private IEnumerator _GetHeadPortriatIEnumer(string url)
    //{
    //    url = headPortriat.headPortraitUrl;
    //    // Debug.Log(_heaPortrat.headPortraitUrl);
    //    // Debug.Log("Function _GetHeadPortriatIEnumer(): GetThe headportriat.");
    //    if (_heaPortrat.headPortraitTexture == null)
    //    {
    //        _heaPortrat.headPortraitTexture = new Texture2D(256, 256);
    //    }
    //    WWW www = new WWW(url);
    //    yield return www;
    //    if (string.IsNullOrEmpty(www.error))
    //    {
    //        _heaPortrat.headPortraitTexture = www.texture;
    //        _heaPortrat.headSprite = Sprite.Create(_heaPortrat.headPortraitTexture,
    //            new Rect(0, 0, headPortriat.headPortraitTexture.width, headPortriat.headPortraitTexture.height),
    //            new Vector2(0.5f, 0.5f)
    //            );
    //        _headPortriatObj.GetComponent<Renderer>().material.mainTexture = _heaPortrat.headPortraitTexture;//本行在未来需要新开一个方法，关于用户头像显示的方法！！！！
    //        _headPotriatMap.GetComponent<Renderer>().material.mainTexture = _heaPortrat.headPortraitTexture;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Function _GetHeadPortriatIEnumer(): www.err " + www.error);
    //        if (_restartCount >= 3)
    //        {
    //            yield return new WaitForSeconds(3f);
    //            _GetTheHeadPortriat();
    //            ++_restartCount;
    //        }
    //    }
    //    www.Dispose();
    //}
    // int _restartCount = 0;


    public void GetTheHeadPortriat()
    {
        if (_heaPortrait != null)
            if (_heaPortrait.headPortraitTexture == null)
            {
                if (string.IsNullOrEmpty(_heaPortrait.headPortraitUrl))
                {
                    Debug.Log("Fuction _GetTheHeadPortriat(): The head portriat url is null.");
                    return;
                }
                StartCoroutine(m_QyUpdateHeadFromHttp());
                PlayerController.GetHeadPortriatFromHttp(this);
            }
            else
                Debug.LogWarning("图片已经下载完成，重复调用!!!!!");
    }
    //------   头像加载  -------
    public bool isLoadHeadTexture = false;
    public byte[] headBuffer = null;
    public bool isHeadTextureCreated = false;
    private IEnumerator m_QyUpdateHeadFromHttp()
    {
        yield return new WaitUntil(() => isLoadHeadTexture);
        //TODO:可以缓冲一帧


        Texture2D m_HeadTextureFromHttp = null;
        // var startTime = DateTime.Now;

        m_HeadTextureFromHttp = new Texture2D(2, 2);
        var isLoadImage = m_HeadTextureFromHttp.LoadImage(headBuffer);
        if (!isLoadImage)
        {
            Debug.LogWarning("Unity不支持要下载的头像图片格式：" + isLoadImage);
        }

        _heaPortrait.headPortraitTexture = m_HeadTextureFromHttp;
        _heaPortrait.headSprite = Sprite.Create(m_HeadTextureFromHttp,
            new Rect(0, 0, m_HeadTextureFromHttp.width, m_HeadTextureFromHttp.height),
            new Vector2(0.5f, 0.5f)
            );
        // var endTime = DateTime.Now;

        //Debug.LogWarning("加载图片的路径：" + _heaPortrait.headPortraitUrl + "\n"
        //                 + "耗时：" + (endTime - startTime).TotalMilliseconds);

        _headPortriatObj.GetComponent<Renderer>().material.mainTexture = _heaPortrait.headPortraitTexture;
        _headPotriatMap.GetComponent<Renderer>().material.mainTexture = _heaPortrait.headPortraitTexture;
        //2017/03/13 资源释放
        headBuffer = null;
    }


    #endregion



    float staySpeed = 1f;

#if ISCAR
    float runInSpeed = 0.24f;
    float runOutSpeed = 0.8f;

#else
    float runInSpeed = 1f / 2f;
    float runOutSpeed = 1f / 2f;
#endif


    bool stay = false;
    bool runIn = false;
    bool runOut = false;
    Vector3 startPos = Vector3.zero;

    public bool itCanMove = false;
    private void _UpdatePosition()
    {
        if (itCanMove)
        {
            this.transform.forward = playerParent.transform.forward;
            Vector3 pos = this.transform.localPosition;
            startPos.x = pos.x;
            startPos.y = pos.y;
#if ISCAR
            startPos.y = -0.4f;
#endif

            startPos.z = pos.z;
            float s = 1;
            if (stay)
            {
                this.transform.localPosition = Vector3.Lerp(startPos, moveTarget, Time.deltaTime * staySpeed);

                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, playerParent.transform.localRotation, Time.fixedDeltaTime * staySpeed * 0.8f);
            }
            else if (runIn)
            {
                this.transform.localPosition = Vector3.Lerp(startPos, moveTarget, Time.deltaTime * runInSpeed);
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, playerParent.transform.localRotation, Time.fixedDeltaTime * runInSpeed * 0.8f);
                s = 1.4f;
            }
            else if (runOut)
            {
                this.transform.localPosition = Vector3.Lerp(startPos, moveTarget, Time.deltaTime * runOutSpeed);
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, playerParent.transform.localRotation, Time.fixedDeltaTime * runOutSpeed * 0.8f);
                s = 0.7f;
            }

            int tempcur = _currentRankTag;
            if (tempcur > 10)
                tempcur = 10;
#if ISCAR
            if ((startPos - moveTarget).magnitude < 0.05f && _currentRankTag > 4)
#else
            _animator.speed = 1.3f - 0.05f * (float)tempcur;
            if ((startPos - moveTarget).magnitude < 0.05f && _currentRankTag > 8)
#endif
            {
                if (_boxFbx.activeSelf)
                {
                    _headPortriatObj.transform.parent.gameObject.SetActive(false);
                    _boxFbx.SetActive(false);
                }
            }
            else
            {
                if (!_boxFbx.activeSelf)
                {
                    _headPortriatObj.transform.parent.gameObject.SetActive(true);
                    _boxFbx.SetActive(true);
                }
            }
#if ISCAR

            if (lookat)
                wheels[0].right = wheels[1].right = Vector3.Lerp(transform.right, Forward.right, 0.4f);
            else
                for (int i = 0; i < wheels.Length; i++)
                {
                    //if (i <= 1)
                    //{
                    //    wheels[i].forward= Vector3.Lerp(wheels[i].position, playerParent.transform.position, 0.05f);
                    //}
                    wheels[i].Rotate(new Vector3(1, 0, 0), 14 * s, Space.Self);

                    //Vector3 local = wheels[i].localRotation.eulerAngles;

                    //wheels[i].RotateAround(wheels[i].TransformPoint(wheels[i].GetComponent<MeshFilter>().mesh.bounds.center), wheels[i].TransformDirection(1, 0, 0), 5);


                }
            lookat = !lookat;
#endif
        }
#if ISCAR
        EndSlip();
#endif
    }
    private bool lookat = false;

    public bool slip = false;
    public Transform endpoint;
    public Transform Forward;

    public void EndSlip()
    {
        if (slip)
        {
            this.transform.forward = endpoint.forward;
            Vector3 pos = this.transform.position;
            startPos.x = pos.x;
            startPos.y = pos.y;
            startPos.y = -0.4f;

            startPos.z = pos.z;


            this.transform.position = Vector3.Lerp(startPos, endpoint.position, Time.deltaTime * 0.04f);

            //this.transform.rotation = Quaternion.Lerp(this.transform.rotation, endpoint.rotation, Time.fixedDeltaTime * 0.04f);

            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].Rotate(new Vector3(1, 0, 0), 10, Space.Self);
                //Vector3 local = wheels[i].localRotation.eulerAngles;
                //wheels[i].forward= Vector3.Lerp(wheels[i].position, playerParent.transform.position, 0.7f);
                //Debug.Log(wheels[i].GetComponent<MeshFilter>().mesh.bounds.center);
                //wheels[i].RotateAround(wheels[i].TransformPoint(wheels[i].GetComponent<MeshFilter>().mesh.bounds.center), wheels[i].TransformDirection(1, 0, 0), 5);

            }
        }

    }


#if ISCAR
    float delta_z_min = 0.15f;
    float delta_z_max = 0.7f;
#else
    float delta_z_min = 0.3f;
    float delta_z_max = 0.6f;
#endif
    public void RunToTargetPosition()
    {
        stay = true;
        runIn = false;
        runOut = false;
        //Debug.Log("RunToTargetPosition targetPosition: banner "
        //                                            + targetPosition + " ("
        //                                            + targetPosition.x + ", "
        //                                            + targetPosition.z + ")"
        //                                            + targetPosition.y + " ,"
        //          );
        Vector3 targetPos;
#if ISCAR
        if (_currentRankTag <= 2)
#else
        if (_currentRankTag <= 3)
#endif
            targetPos = new Vector3(_leftSizex + _trackTag * _xSpan, -0.4f, delta_z_max * 4 - _currentRankTag * delta_z_max);
        else
            targetPos = new Vector3(_leftSizex + _trackTag * _xSpan, -0.4f, delta_z_min * 4 - _currentRankTag * delta_z_min);
        moveTarget = targetPos;
        //_headPortriatObj.transform.parent.gameObject.SetActive(true);

        //this.transform.DOLocalMove(targetPos, 1f);

    }

    public void RunInScreen()
    {
        stay = false;
        runIn = true;
        runOut = false;
        //this.transform.SetParent(playerParent.transform);
        Vector3 targetPos;
#if ISCAR
        this.transform.localPosition = new Vector3(_leftSizex + _trackTag * _xSpan, -0.4f, -6);
        if (_currentRankTag <= 1)
#else
        if (_currentRankTag <= 3)
#endif
        {
            targetPos = new Vector3(_leftSizex + _trackTag * _xSpan, -0.4f, delta_z_max * 4 - _currentRankTag * delta_z_max);
        }

        else
            targetPos = new Vector3(_leftSizex + _trackTag * _xSpan, -0.4f, delta_z_min * 4 - _currentRankTag * delta_z_min);
        //this.transform.DOLocalMove(targetPos, 1f);
        moveTarget = targetPos;
        //_headPortriatObj.transform.parent.gameObject.SetActive(true);
        //_boxFbx.SetActive(true);
    }
    public void RunOutScreen()
    {
        stay = false;
        runIn = false;
        runOut = true;
        //Debug.Log("RunOutScreen targetPosition ->->->->->->" + this._nickName + "("
        //                                              + this.transform.localPosition.x + ","
        //                                              + this.transform.localPosition.y + ","
        //                                              + this.transform.localPosition.z + ")"
        //                                              );


#if ISCAR
        Vector3 targetPos = new Vector3(this.transform.localPosition.x, -0.4f, -10f);
#else
        Vector3 targetPos = new Vector3(this.transform.localPosition.x, -0.4f, -10f);
#endif
        moveTarget = targetPos;
    }
}
