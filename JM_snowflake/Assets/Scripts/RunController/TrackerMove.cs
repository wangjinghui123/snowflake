using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WaypointProgressTracker))]
public class TrackerMove : MonoBehaviour
{
    /// <summary>
    /// 拖动主体
    /// </summary>
    private Transform _charactorBody;
    /// <summary>
    /// 移动目标点
    /// </summary>
    private Transform _target;

    private WaypointProgressTracker _tracker;

    void Awake()
    {
        _Initialized();
    }

    public bool isStart;

    void FixedUpdate()
    {
        //if (Input.GetMouseButtonUp(0))
        //    isStart = !isStart;
        if (isStart)
            _UpdateCharactersDirection();
    }


    /// <summary>
    /// 初始化
    /// </summary>
    private void _Initialized()
    {
        _charactorBody = this.transform;
        _tracker = _charactorBody.GetComponent<WaypointProgressTracker>();
        _target = _tracker.target;

    }

    /// <summary>
    /// 玩家位置更新
    /// </summary>
    private void _UpdateCharactersDirection()
    {
        _charactorBody.forward = _target.position - _charactorBody.position;
        _charactorBody.position = Vector3.Lerp(_charactorBody.position, new Vector3(_target.position.x, _charactorBody.position.y, _target.position.z), Time.fixedDeltaTime * 1.5f);
        _charactorBody.rotation = Quaternion.Lerp(_charactorBody.rotation, _target.rotation, Time.fixedDeltaTime);
    }

}
